using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
namespace Clinique_Projet.Modal
{
    public class AnalyseClass
    {
        public int IdAnalyse { get; set; }
        public string NomAnalyse { get; set; }
        public string TypeAnalyse { get; set; }
       

        public AnalyseClass() { }

        public AnalyseClass(int idAnalyse, string nomAnalyse, string typeAnalyse)
        {
            this.IdAnalyse = idAnalyse;
            this.NomAnalyse = nomAnalyse;
            this.TypeAnalyse = typeAnalyse;
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
                        commande.CommandText = "SELECT MAX(id_Analyse) FROM Analyse;";
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
                                cmd.Connection = con;

                        string sql = "INSERT INTO Analyse (id_Analyse,Nom_Analyse, Type_Analyse_id) " +
                            "VALUES (@id_Analyse,@nom_analyse, @type_analyse);";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_Analyse", IdAnalyse);
                        cmd.Parameters.AddWithValue("@nom_analyse", NomAnalyse);
                        cmd.Parameters.AddWithValue("@type_analyse", Convert.ToInt32(TypeAnalyse));
                        cmd.ExecuteNonQuery();
                            }
                    con.Close();
                    return true;
                    }
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
                        string sql = "update  Analyse " +
                            " set Nom_Analyse=@nom_analyse , Type_Analyse_id=@type_analyse " +
                            " where id_Analyse=@idAnalyse;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idAnalyse", IdAnalyse);
                        cmd.Parameters.AddWithValue("@nom_analyse", NomAnalyse);
                        cmd.Parameters.AddWithValue("@type_analyse", Convert.ToInt32(TypeAnalyse));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // delete Analyse
        public static bool DeleteAnalyse(int IdAnalyse)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Analyse " +
                            " where id_Analyse=@idAn ; ";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idAn", IdAnalyse);
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

        //select  analyse 
        public static AnalyseClass Select_Analyse(int id_analyse)
        {
            AnalyseClass  A = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select a.id_Analyse,a.Nom_Analyse,t.Nom_TypeAN from Analyse a ,Type_Analyse t " +
                                        "  where  a.id_Analyse=@IdAnalyse and  a.Type_Analyse_id=t.id_TypeAN ;";
                    commande.Parameters.AddWithValue("@IdAnalyse", id_analyse);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A =new AnalyseClass((int)reader[0],(string)reader[1], (string)reader[2]);  
                    }
                    reader.Close();
                }
                con.Close();
            }
            return A;
        }

        //display  analyse by type
        public static ObservableCollection<AnalyseClass> Display_Analyse_Type()
        {
            ObservableCollection<AnalyseClass> A = new ObservableCollection<AnalyseClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select a.id_Analyse,a.Nom_Analyse,t.Nom_TypeAN from Analyse a,Type_Analyse t" +
                                        " where a.Type_Analyse_id=t.id_TypeAN order by id_Analyse desc;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A.Add(new AnalyseClass
                        {
                            IdAnalyse = (int)reader[0],
                            NomAnalyse = (string)reader[1],
                            TypeAnalyse =(string)reader[2]
                        });
                    }
                    reader.Close();
                }
                return A;
            }
        }

        //display  analyse
        public static ObservableCollection<AnalyseClass> Display_Analyse()
        {
            ObservableCollection<AnalyseClass> A = new ObservableCollection<AnalyseClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Analyse;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A.Add(new AnalyseClass
                        {
                            IdAnalyse = (int)reader[0],
                            NomAnalyse = (string)reader[1],
                            TypeAnalyse = reader[2].ToString()
                        });
                    }
                    reader.Close();
                }
                return A;
            }
        }

        //display  analyse by type analyse
        public static ObservableCollection<AnalyseClass> Display_Analyse_ByType(int idtype)
        {
            ObservableCollection<AnalyseClass> A = new ObservableCollection<AnalyseClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Analyse where Type_Analyse_id=@idtype;";
                    commande.Parameters.AddWithValue("@idtype", idtype);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        A.Add(new AnalyseClass
                        {
                            IdAnalyse = (int)reader[0],
                            NomAnalyse = (string)reader[1],
                            TypeAnalyse = reader[2].ToString()
                        });
                    }
                    reader.Close();
                }
                return A;
            }
        }

    }
}
