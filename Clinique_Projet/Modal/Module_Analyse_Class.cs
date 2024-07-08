using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
   public class Module_Analyse_Class
    {
        public int  Id_Module_Analyse { get; set; }
        public string Nom_Module_Analyse { get; set; }

        public Module_Analyse_Class() { }
        public Module_Analyse_Class(int id,string nom)
        { 
            this.Nom_Module_Analyse = nom;
            this.Id_Module_Analyse = id;
        }

        // add module de Analyse
        public bool Add_Module_Analyse()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                                cmd.Connection = con;
                                cmd.CommandText = " INSERT INTO Module_Bilan(Nom_Module_Bilan) " +
                                    " VALUES (@Nom_Module_Bilan); " ;
                                cmd.Parameters.AddWithValue("@Nom_Module_Bilan", Nom_Module_Analyse);
                                cmd.ExecuteNonQuery();
                    }
                }
                return true;
                }
            catch (Exception)
            {
                return false;
            }
        }

        // update module de Analyse
        public bool Update_Module_Analyse()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " update  Module_Bilan set Nom_Module_Bilan=@Nom_Module_Bilan " +
                            " where Id_Module_Bilan=@Id_Module_Bilan  ; ";
                        cmd.Parameters.AddWithValue("@Id_Module_Bilan", Id_Module_Analyse);
                        cmd.Parameters.AddWithValue("@Nom_Module_Bilan", Nom_Module_Analyse);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // delete module de Analyse
        public static bool Delete_Module_Analyse(int id)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " delete  Module_Bilan  " +
                            " where Id_Module_Bilan=@Id_Module_Bilan  ; ";
                        cmd.Parameters.AddWithValue("@Id_Module_Bilan", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
                    commande.CommandText = "select MAX(Id_Module_Bilan) from Module_Bilan;";
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

        // add les  Analyse dans le module
        public static void Add_Analyse_on_module(int id_module,int id_analyse)
        {
             using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " INSERT INTO Bilan_Module " +
                            " VALUES (@Module_Bilan_id,@Analyse_id); ";
                        cmd.Parameters.AddWithValue("@Module_Bilan_id", id_module);
                        cmd.Parameters.AddWithValue("@Analyse_id", id_analyse);
                        cmd.ExecuteNonQuery();
                    }
                }
        }

        // delete module de Analyse
        public static bool Delete_Analyse_on_module(int id_module,int id_analyse)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " delete  Bilan_Module  " +
                            " where Module_Bilan_id=@Module_Bilan_id and Analyse_id=@Analyse_id ; ";
                        cmd.Parameters.AddWithValue("@Module_Bilan_id", id_module);
                        cmd.Parameters.AddWithValue("@Analyse_id", id_analyse);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //select module analyse
        public static ObservableCollection<Module_Analyse_Class> Display_Module_Analyse()
        {
            ObservableCollection<Module_Analyse_Class> list = new ObservableCollection<Module_Analyse_Class>();
            list.Add(new Module_Analyse_Class { Id_Module_Analyse = -1, Nom_Module_Analyse = ""});
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Id_Module_Bilan,Nom_Module_Bilan from Module_Bilan ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Module_Analyse_Class
                        {
                            Id_Module_Analyse = (int)reader[0],
                            Nom_Module_Analyse = (string)(reader[1]),
                        });
                    }
                    reader.Close();
                }
            }
            return list;
        }

        //display analyse et leur type a partir de module
        public static ObservableCollection<AnalyseClass> Display_Analyse_of_module(int id_module)
        {
            ObservableCollection<AnalyseClass> list = new ObservableCollection<AnalyseClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "SELECT A.id_Analyse,A.Nom_Analyse, T.Nom_TypeAN " +
                                  " FROM Bilan_Module BM" +
                                  " JOIN Analyse A ON BM.Analyse_id = A.id_Analyse " +
                                  " JOIN Type_Analyse T ON A.Type_Analyse_id = T.id_TypeAN" +
                                  " WHERE BM.Module_Bilan_id =@id_module;";
                    commande.Parameters.AddWithValue("@id_module", id_module);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new AnalyseClass
                        {
                            IdAnalyse = (int)reader[0],
                            NomAnalyse = (string)(reader[1]),
                            TypeAnalyse = (string)(reader[2])
                        });
                    }
                    reader.Close();
                }
            }
            return list;
        }

        //display les id des analyse  d'un module
        public static ObservableCollection<int> Display_Analyse_Module(int id_module)
        {
            ObservableCollection<int> p = new ObservableCollection<int>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Analyse_id from Bilan_Module where Module_Bilan_id=@Module_Bilan_id;";
                    commande.Parameters.AddWithValue("@Module_Bilan_id", id_module);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add((int)reader[0]);
                    }
                    reader.Close();
                }
                return p;
            }
        }

    }
}
