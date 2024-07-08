using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
   public class Medicament_Class
    {
        public int IdMedcament { get; set; }
        public string NomMedcament { get; set; }
        public string CatMedcament { get; set; }

        public Medicament_Class() { }

        public Medicament_Class(int idMedc, string nomMedc, string CatMedc)
        { 
            this.IdMedcament = idMedc;
            this.NomMedcament = nomMedc;
            this.CatMedcament = CatMedc;
        }


        public static int GetLast_Id()
        {
            int id = 0;
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var commande = new SqlCommand())
                    {
                        commande.Connection = con;
                        commande.CommandText = "SELECT MAX(id_Medicament) FROM Medicament;";
                        var result = commande.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return id;
        }
     
        // add medicament
        public bool AddMedicament()
        {
            try
            {             
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Medicament(Nom_Medicament, CatMedicament_id, id_Medicament) " +
                            " values (@nom_medicament,@type_medicament,@id_med ) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@nom_medicament", NomMedcament);
                        cmd.Parameters.AddWithValue("@id_med", IdMedcament);
                        cmd.Parameters.AddWithValue("@type_medicament", Convert.ToInt32(CatMedcament));
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
      
        // update medicament
        public bool UpdateMedicament()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Medicament " +
                            " set Nom_Medicament=@nom_medicament , CatMedicament_id=@type_medicament " +
                            " where id_Medicament=@id_Medicament;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_Medicament", IdMedcament);
                        cmd.Parameters.AddWithValue("@nom_medicament", NomMedcament);
                        cmd.Parameters.AddWithValue("@type_medicament", Convert.ToInt32(CatMedcament));
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
       
        // delete medicament
        public static bool DeleteMedicament(int IdMedicament)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Medicament " +
                            " where id_Medicament=@IdMedicament ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@IdMedicament", IdMedicament);
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

        //select  medicament 
        public static Medicament_Class Select_Medicament(int id_medicament)
        {
            Medicament_Class A = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select m.id_Medicament,m.Nom_Medicament,c.Nom_CatMedicament from Medicament m ,Categorie_Medicament c " +
                                          " where m.id_Medicament =@id_Medicament and m.CatMedicament_id = c.id_CatMedicament ; ";
                    commande.Parameters.AddWithValue("@id_Medicament", id_medicament);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A = new Medicament_Class((int)reader[0], (string)reader[1], (string)reader[2]);
                    }
                    reader.Close();
                }
                con.Close();
            }
            return A;
        }


        //display  medicaments
        public static ObservableCollection<Medicament_Class> Display_Medicament()
        {
            ObservableCollection<Medicament_Class> A = new ObservableCollection<Medicament_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select m.id_Medicament,m.Nom_Medicament,c.Nom_CatMedicament" +
                        " from Medicament m , Categorie_Medicament c" +
                        " where CatMedicament_id=id_CatMedicament ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A.Add(new Medicament_Class
                        {
                            IdMedcament = (int)reader[0],
                            NomMedcament = (string)reader[1],
                            CatMedcament = reader[2].ToString()
                        });
                    }
                    reader.Close();
                }
                return A;
            }
        }
      
        //display  medicaments by categorie medicaments
        public static ObservableCollection<Medicament_Class> Display_Medicament_ByCat(int idCat)
        {
            ObservableCollection<Medicament_Class> A = new ObservableCollection<Medicament_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select id_Medicament,Nom_Medicament,CatMedicament_id from Medicament where CatMedicament_id=@idcat Order by  id_Medicament desc;";
                    commande.Parameters.AddWithValue("@idcat", idCat);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A.Add(new Medicament_Class
                        {
                            IdMedcament = (int)reader[0],
                            NomMedcament = (string)reader[1],
                            CatMedcament= reader[2].ToString()
                        });
                    }
                    reader.Close();
                }
                return A;
            }
        }
    }
}
