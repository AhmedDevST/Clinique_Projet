using Clinique_Projet.connectionDb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class AssuranceClass
    {
        public int IdAssurance { get; set; }
        public string NomAssurance { get; set; }    
        public AssuranceClass() {  }
        public AssuranceClass(int id,string? nom) 
        { 
            this.IdAssurance = id;
            this.NomAssurance = nom;
        }
        public AssuranceClass(int id)
        {
            this.IdAssurance = id;
        }

        public AssuranceClass(string nom)
        {
            this.NomAssurance = nom;
        }
        // Add Assurance
        public bool Add_Assurance()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Assurance( Nom_Assurance ) " +
                            " values (@Nom_Assurance ) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Nom_Assurance", NomAssurance);
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

        // Update Assurance
        public bool Update_Assurance()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Assurance " +
                            " set Nom_Assurance=@Nom_Assurance " +
                            " where id_Assurance=@id_Assurance;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_Assurance", IdAssurance);
                        cmd.Parameters.AddWithValue("@Nom_Assurance", NomAssurance);
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

        // delete assurance
        public static bool Delete_Assurance(int Id)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Assurance " +
                            " where id_Assurance=@id_Assurance;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_Assurance", Id);
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

        //select Asssurance(Combobox)
        public static List<KeyValuePair<int, String>> DisplayAssurance()
        {
            List<KeyValuePair<int, String>> ListAssurance = new List<KeyValuePair<int, string>>();

            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select* from Assurance";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        ListAssurance.Add(new KeyValuePair<int, string>((int)reader[0], (String)reader[1]));
                    }
                    reader.Close();
                }
            }
            return ListAssurance;
        }
   
        //select Agent de assurance
        public static ObservableCollection<AssuranceClass> DisplayAssurance1()
        {
            ObservableCollection<AssuranceClass> list = new ObservableCollection<AssuranceClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Assurance where Nom_Assurance!='Non assure';";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new AssuranceClass
                        {
                            IdAssurance = (int)reader[0],
                            NomAssurance = (string)(reader[1])
                        });
                    }
                    reader.Close();
                }
                return list;
            }
        }

        //slecte nom d'assurance
        public static string NamofAssurance(int id)
        {
            string NameAssurance = "";
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Assurance where id_Assurance=@id";
                    commande.Parameters.AddWithValue("@id", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        NameAssurance = reader[1].ToString();
                    }
                    reader.Close();
                }
            }
            return NameAssurance;
        }
   
    }
}
