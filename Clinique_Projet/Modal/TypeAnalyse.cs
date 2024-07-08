using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
   public class TypeAnalyse
    {
        public int ID_TypeBilan {  get; set; }
        public string Nom_TypeBilan { get; set; }

        public TypeAnalyse() { }
        public TypeAnalyse(int id, string nomType) { 
            this.ID_TypeBilan = id;
            this.Nom_TypeBilan= nomType;
        }
        public TypeAnalyse(string nomType)
        {
            this.Nom_TypeBilan = nomType;
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
                        commande.CommandText = "SELECT MAX(id_TypeAN) FROM Type_Analyse;";
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

        // add Analyse
        public bool AddAnalyse()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Type_Analyse( id_TypeAN,Nom_TypeAN ) " +
                           " values (@id_TypeAN,@NomType_analyse ) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_TypeAN", ID_TypeBilan);
                        cmd.Parameters.AddWithValue("@NomType_analyse", Nom_TypeBilan);
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
       
        // update Analyse
        public bool UpdateAnalyse()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql =  "update  Type_Analyse " +
                            " set Nom_TypeAN=@nom_analyse " +
                            " where id_TypeAN=@idTYpe_Analyse;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idTYpe_Analyse", ID_TypeBilan);
                         cmd.Parameters.AddWithValue("@nom_analyse", Nom_TypeBilan);
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
        // delete Analyse
        public static bool DeleteAnalyse(int IdType_Analyse)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Type_Analyse " +   
                            " where id_TypeAN=@idTYpe_Analyse;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idTYpe_Analyse", IdType_Analyse);
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
     
        //display types analyse  
        public static ObservableCollection<TypeAnalyse> Display_TypesAnlyse()
        {
            ObservableCollection<TypeAnalyse> TA = new ObservableCollection<TypeAnalyse>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Type_Analyse order by id_TypeAN desc;";
                     var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        TA.Add(new TypeAnalyse
                        {
                            ID_TypeBilan = (int)reader[0],
                            Nom_TypeBilan = (string)reader[1]
                        });
                    }
                    reader.Close();
                }
                return TA;
            }
        }
    }
}
