using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;


namespace Clinique_Projet.Modal
{
    public class ConsultationClass
    {
        public  int IdConsult { get; set; }
        public string MotifConsult { get; set; }
        public string ExamenClinque_Consult { get; set; }
        public string Diagnostique_Consult { get; set; }
        public DateTime DateConsult { get; set; }  
        public decimal Poids_patient { get; set; }
        public int Taille_patient { get; set; }
        public int Terminer_consult { get; set; }
        public int ID_patient { get; set; }
        public string NomConsult{ get; set; }
        public ConsultationClass() { }

        public ConsultationClass(int id,string motif, DateTime date,string examenc,string diagnostique,
                                decimal p,int t,int terminer,int idp)
        {
            this.IdConsult = id;
            this.DateConsult = date;
            this.MotifConsult = motif;
            this.ExamenClinque_Consult = examenc;
            this.Diagnostique_Consult = diagnostique;
            this.Terminer_consult = terminer;
            this.Poids_patient = p;
            this.Taille_patient = t;
            this.ID_patient = idp;
        }


        // add Consultation
        public bool AddConsultation()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Consultation(Motif_Consult,Date_Consult,ExamenClinque_Consult," +
                            "Diagnostique_Consult,Terminer_Consult,Patient_id,Poids_patient,Taille_patient) " +
                            " values (@motif,@dateConsult,@ExamenClinque,@Diagnostique " +
                            ",@terminer,@patient,@poids,@taille) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@motif", MotifConsult);
                        cmd.Parameters.AddWithValue("@dateConsult", DateConsult);
                        cmd.Parameters.AddWithValue("@ExamenClinque", ExamenClinque_Consult);
                        cmd.Parameters.AddWithValue("@Diagnostique", Diagnostique_Consult);
                        cmd.Parameters.AddWithValue("@terminer", Terminer_consult);
                        cmd.Parameters.AddWithValue("@patient", ID_patient);
                        cmd.Parameters.AddWithValue("@poids", Poids_patient);
                        cmd.Parameters.AddWithValue("@taille", Taille_patient);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        // update consultation
        public void UpdateConsultation()
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    string sql = "update  Consultation  set Motif_Consult=@motif,ExamenClinque_Consult=@ExamenClinque, " +
                            " Diagnostique_Consult=@Diagnostique ,Terminer_Consult=@terminer, " +
                            " Poids_patient=@poids ,Taille_patient=@taille " +
                            " where id_Consult=@idconsult ;";
                       
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@idconsult", IdConsult);
                    cmd.Parameters.AddWithValue("@motif", MotifConsult);
                    cmd.Parameters.AddWithValue("@dateConsult", DateConsult);
                    cmd.Parameters.AddWithValue("@ExamenClinque", ExamenClinque_Consult);
                    cmd.Parameters.AddWithValue("@Diagnostique", Diagnostique_Consult);
                    cmd.Parameters.AddWithValue("@terminer", Terminer_consult);
                    cmd.Parameters.AddWithValue("@poids", Poids_patient);
                    cmd.Parameters.AddWithValue("@taille", Taille_patient);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        // delete Consultation
        public static bool DeleteConsultation(int id)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "delete  Consultation where id_Consult=@id ;"; 
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
     
        //get Id of last 
        public static int GetLast_Id()
        {
            int idCons=0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select MAX(id_Consult) from Consultation;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        idCons = Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                return idCons;
            }
        }
     
        //display consultations of patient
        public static ObservableCollection<ConsultationClass> DisplayConsult_Datagrid(int idp)
        {
            ObservableCollection<ConsultationClass> c = new ObservableCollection<ConsultationClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select id_Consult,Date_Consult,Patient_id " +
                                         " from Consultation where Patient_id=@idp order by Date_Consult desc;";
                    commande.Parameters.AddWithValue("@idp", idp);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        c.Add(new ConsultationClass
                        {
                            IdConsult = (int)reader[0],
                            NomConsult = "Consultation "+ Convert.ToDateTime(reader[1]).ToShortDateString()
                        });
                    }
                    reader.Close();
                }
                return c;
            }
        }
      
        //select consultation
        public static ConsultationClass SelectConsultation(int idc)
        {
            ConsultationClass consultation = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select id_Consult,Motif_Consult,Date_Consult ," +
                        " ExamenClinque_Consult,Diagnostique_Consult,Poids_patient,Taille_patient,Terminer_Consult,Patient_id  " +
                        "from Consultation where id_Consult=@idc;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.Parameters.AddWithValue("@idc", idc);
                    commande.CommandText=sql;
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        consultation = new ConsultationClass(
                            (int)reader[0],
                             (string)reader[1],
                              (DateTime)reader[2],
                              (string)reader[3],
                             (string)reader[4],
                              (decimal)reader[5],
                               (int)reader[6],
                                (int)reader[7],
                                (int)reader[8]              );
                    }
                    reader.Close();
                }
                return consultation;
            }
        }

        // select motifs,diagnostique,examen clinique
        public static ObservableCollection<string> Display_champs_consultation()
        {
            ObservableCollection<string> c = new ObservableCollection<string>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Motif_Consult,Diagnostique_Consult,ExamenClinque_Consult " +
                                         " from Consultation ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        c.Add(new string(reader[0].ToString()));
                        c.Add(new string(reader[1].ToString()));
                        c.Add(new string(reader[2].ToString()));
                    }
                    reader.Close();
                }
                return c;
            }
        }

    }
}
