using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class RendezVous
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public TimeSpan heure { get; set; }
        public string type { get; set; }
        public int id_patient { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string description { get; set; }
        public string sexe { get; set; }


        public RendezVous() { }
      

        // type signifie si je voudrais afficher des sticky note ou tout les rendez vous
        public static ObservableCollection<RendezVous> DisplayRendezVous(int T)
        {
            ObservableCollection<RendezVous> p = new ObservableCollection<RendezVous>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    if (T == -1)
                        commande.CommandText = "Select R.id,P.Nom_patient,P.Prenom_patient, R.date,R.heure,R.type, R.description, P.Sex " +
                                "from RendezVous R join Patient P " +
                                "on R.id_patient=P.id_patient ";
                    else if (T >= 0)
                    {


                      commande.CommandText = "Select R.id,P.Nom_patient,P.Prenom_patient, R.date,R.heure,R.type, R.description, P.Sex " +
                            "from RendezVous R Join Patient P  " +
                            " on R.id_patient=P.id_patient " +
                            " where R.id=@T";
                        commande.Parameters.AddWithValue("@T", T);
                    }
                    else if (T == -2) commande.CommandText = "Select R.id,P.Nom_patient,P.Prenom_patient, R.date,R.heure,R.type, R.description, P.Sex"+
                            " from RendezVous R Join Patient P "+
                            " on R.id_patient = P.id_patient"+
                             " WHERE CAST(R.date AS DATE) = CAST(GETDATE() AS DATE);";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new RendezVous
                        {
                            id = (int)reader[0],
                            date = (DateTime)reader[3],
                            heure = (TimeSpan)reader[4],
                            type = (string)reader[5],
                            prenom = (string)reader[2],
                            nom= (string)reader[1],                            
                            sexe = (string)reader[7],
                            description = (string)reader[6]
                        });
                    }
                    reader.Close();
                }
                return p;
            }
        }

        public static int get_patient(int id_rendezVous)
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "SELECT id_patient FROM RendezVous where Id=@id_rendezVous";
                    commande.Parameters.AddWithValue("@id_rendezVous", id_rendezVous);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        id_rendezVous = (int)reader[0];
                    }
                    reader.Close();
                }
                return id_rendezVous;

            }
        }

        public static void ajouter_RendezVous2(DateTime? date, string time, string type, int id_p,string description)
        {
            // récuperer l'id d'aprés le combobox
            int c = id_p;
            // verifier si la date est choisie ou non

            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    try
                    {
                        commande.Connection = con;
                        commande.CommandText = "INSERT INTO RendezVous(date,heure,type,id_patient,description) Values(@date,@heure,@type,@id_p,@description)";
                        commande.Parameters.AddWithValue("@date", date);
                        commande.Parameters.AddWithValue("@heure", Convert.ToDateTime(time));
                        commande.Parameters.AddWithValue("@type", type);
                        commande.Parameters.AddWithValue("@id_p", c);
                        commande.Parameters.AddWithValue("@description", description);

                        commande.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception )
                    { 
                    }
                }
            }
        }

        public static void ajouter_RendezVous( DateTime? date, string time, string type, string id_p,string description)
        {
            // récuperer l'id d'aprés le combobox
            int c=1; string id="";
            while (id_p[c].ToString()!= ",")
            {
                id = id + id_p[c];
                c++;
            }
             c=Convert.ToInt32(id);
            // verifier si la date est choisie ou non
           
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    try {
                        commande.Connection = con;
                        commande.CommandText = "INSERT INTO RendezVous(date,heure,type,id_patient,description) Values(@date,@heure,@type,@id_p,@description)";
                        commande.Parameters.AddWithValue("@date", date);
                        commande.Parameters.AddWithValue("@heure",Convert.ToDateTime(time));
                        commande.Parameters.AddWithValue("@type", type);
                        commande.Parameters.AddWithValue("@id_p", c);
                        commande.Parameters.AddWithValue("@description", description);
                        
                        commande.ExecuteNonQuery();
                        con.Close();
                    }catch(Exception)
                    { 
                    }
                }
            }
        }
        public static void ModifierReendezVous(int id,DateTime? date,string type,string description,string time)
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    try
                    {
                        commande.Connection = con;
                        commande.CommandText = "Update RendezVous set date=@date, type=@type, description=@description, heure=@time where id=@id";
                        commande.Parameters.AddWithValue("@date", Convert.ToDateTime(date));
                        commande.Parameters.AddWithValue("@type", type);
                        commande.Parameters.AddWithValue("@time", Convert.ToDateTime(time));
                        commande.Parameters.AddWithValue("@description", description);
                        commande.Parameters.AddWithValue("@id", id);
                        commande.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception) {}
                }
            }
        }
        public static void supprimerRendezVous(int id)
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    try
                    {
                        commande.Connection = con;
                        commande.CommandText = "DELETE FROM RendezVous WHERE id=@id;";
                        commande.Parameters.AddWithValue("@id", id);
                        commande.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception) {}
                }
            }
        }
        public static bool date_heure_valide(string time,DateTime date)
        {
            int i = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;

                    commande.CommandText=" DECLARE @heure_manuelle time = @time;" +
                     "SELECT         CASE " +
                     "WHEN DATEPART(hour, heure) = DATEPART(hour, @heure_manuelle) " +
                       "  AND DATEPART(minute, heure) = DATEPART(minute, @heure_manuelle)" +
                    " THEN 'false' " +
                    " ELSE 'true' " +
                 "END AS resultat FROM  RendezVous where  date=@date";
                        
                    
                    commande.Parameters.AddWithValue("@time", time);
                    commande.Parameters.AddWithValue("@date", date);
                    var reader = commande.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        if (reader[0].ToString() == "false")
                        {
                            i++;
                            if (i == 2) return false;


                        }
                    }
                    reader.Close();

                }
            }
             return true;
        }
        public static bool date_heure_valide1(string time, DateTime date)
        {
            int i = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;

                    commande.CommandText = " DECLARE @heure_manuelle time = @time;" +
                     "SELECT         CASE " +
         "WHEN DATEPART(hour, heure) = DATEPART(hour, @heure_manuelle) " +
           "  AND DATEPART(minute, heure) = DATEPART(minute, @heure_manuelle)" +
        " THEN 'false' " +
        " ELSE 'true' " +
     "END AS resultat FROM  RendezVous where  date=@date";


                    commande.Parameters.AddWithValue("@time", time);
                    commande.Parameters.AddWithValue("@date", date);
                    var reader = commande.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[0].ToString() == "false")
                        {
                            i++;
                            if (i == 2) return false;
                        }
                    }
                    reader.Close();

                }
            }

            return true;
        }

        //get Id of last row
        public static int GetLast_Id()
        {
            int id = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = " select MAX(Id) from RendezVous;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                return id;
            }
        }
    }
}
