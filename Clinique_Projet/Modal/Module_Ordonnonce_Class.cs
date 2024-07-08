using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Module_Ordonnonce_Class
    {
        public int Id_Module_Ordonnonce { get; set; }
        public string Nom_Module_Ordonnonce { get; set; }

        public Module_Ordonnonce_Class() { }
        public Module_Ordonnonce_Class(int id,string nom) 
        {
            this.Id_Module_Ordonnonce = id;
            this.Nom_Module_Ordonnonce = nom;
        }

        // add module de ordonnonxe
        public bool Add_Module_Ordonnonce()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " INSERT INTO Module_Ordonnonce(Nom_Module_Ordonnonce) " +
                            " VALUES (@Nom_Module_Ordonnonce); ";
                        cmd.Parameters.AddWithValue("@Nom_Module_Ordonnonce", Nom_Module_Ordonnonce);
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

        // Update module de ordonnonxe
        public bool Update_Module_Ordonnonce()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " update  Module_Ordonnonce set Nom_Module_Ordonnonce=@Nom_Module_Ordonnonce " +
                                     " where Id_Module_Ordonnonce=@Id_Module_Ordonnonce; ";
                        cmd.Parameters.AddWithValue("@Nom_Module_Ordonnonce", Nom_Module_Ordonnonce);
                        cmd.Parameters.AddWithValue("@Id_Module_Ordonnonce", Id_Module_Ordonnonce);
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

        // Delete module de ordonnonxe
        public static bool Delete_Module_Ordonnonce(int id_module)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " delete  Module_Ordonnonce  " +
                                     " where Id_Module_Ordonnonce=@Id_Module_Ordonnonce; ";
                        cmd.Parameters.AddWithValue("@Id_Module_Ordonnonce", id_module);
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
                    commande.CommandText = "select MAX(Id_Module_Ordonnonce) from Module_Ordonnonce;";
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

        // add   medicament dans le module
        public static void Add_Medicament_on_module(int id_module, int id_medicament)
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = " INSERT INTO Ordonnance_Module " +
                        " VALUES (@Module_Ordonnonce_id,@Medicament_id); ";
                    cmd.Parameters.AddWithValue("@Module_Ordonnonce_id", id_module);
                    cmd.Parameters.AddWithValue("@Medicament_id", id_medicament);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // delete   Analyse dans le module
        public static bool Delete_Medicament_on_module(int id_module, int id_medicament)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = " delete  Ordonnance_Module " +
                            " where Module_Ordonnonce_id=@Module_Ordonnonce_id and Medicament_id=@Medicament_id; ";
                        cmd.Parameters.AddWithValue("@Module_Ordonnonce_id", id_module);
                        cmd.Parameters.AddWithValue("@Medicament_id", id_medicament);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        //select module analyse
        public static ObservableCollection<Module_Ordonnonce_Class> Display_Module_Ordonnonce()
        {
            ObservableCollection<Module_Ordonnonce_Class> list = new ObservableCollection<Module_Ordonnonce_Class>();
            list.Add(new Module_Ordonnonce_Class { Id_Module_Ordonnonce = -1, Nom_Module_Ordonnonce = "" });
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Id_Module_Ordonnonce,Nom_Module_Ordonnonce from Module_Ordonnonce ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Module_Ordonnonce_Class
                        {
                            Id_Module_Ordonnonce = (int)reader[0],
                            Nom_Module_Ordonnonce = (string)(reader[1]),
                        }) ;
                    }
                    reader.Close();
                }
            }
            return list;
        }

        //display medicament et leur type a partir de module
        public static ObservableCollection<Medicament_Class> Display_Medicament_of_module(int id_module)
        {
            ObservableCollection<Medicament_Class> list = new ObservableCollection<Medicament_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "SELECT A.id_Medicament,A.Nom_Medicament, T.Nom_CatMedicament " +
                                  " FROM Ordonnance_Module BM" +
                                  " JOIN Medicament A ON BM.Medicament_id = A.id_Medicament " +
                                  " JOIN Categorie_Medicament T ON A.CatMedicament_id = T.id_CatMedicament" +
                                  " WHERE BM.Module_Ordonnonce_id =@id_module;";
                    commande.Parameters.AddWithValue("@id_module", id_module);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Medicament_Class
                        {
                            IdMedcament = (int)reader[0],
                            NomMedcament = (string)(reader[1]),
                            CatMedcament = (string)(reader[2])
                        });
                    }
                    reader.Close();
                }
            }
            return list;
        }

        //display les module ordonnonce 
        public static ObservableCollection<int> Display_Ordonnonce_Module(int id_module)
        {
            ObservableCollection<int> p = new ObservableCollection<int>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select a.Medicament_id from Ordonnance_Module a where a.Module_Ordonnonce_id=@Module_Ordonnonce_id ;";
                    commande.Parameters.AddWithValue("@Module_Ordonnonce_id", id_module);
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
