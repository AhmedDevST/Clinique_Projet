using Clinique_Projet.connectionDb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class GroupSangClass
    {
        public int IdGroupSang { get; set; }
        public string NonmGroupSang { get; set; }
        public GroupSangClass() { }
        public GroupSangClass(int id, string nom)
        {
            this.IdGroupSang = id;
            this.NonmGroupSang = nom;
        }
        // Add Assurance
        public bool Add_GroupSang()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into GroupSang( Nom_GroupSang ) " +
                            " values (@Nom_GroupSang ) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Nom_GroupSang", NonmGroupSang);
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
        public bool Update_GroupSang()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  GroupSang " +
                            " set Nom_GroupSang=@Nom_GroupSang " +
                            " where id_GroupSang=@id_GroupSang;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_GroupSang", IdGroupSang);
                        cmd.Parameters.AddWithValue("@Nom_GroupSang", NonmGroupSang);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }

        }

        // delete assurance
        public static bool Delete_GroupSang(int Id)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  GroupSang " +
                            " where id_GroupSang=@id_GroupSang;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_GroupSang", Id);
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

        //select Agent de assurance
        public static ObservableCollection<GroupSangClass> Display_GroupSang()
        {
            ObservableCollection<GroupSangClass> list = new ObservableCollection<GroupSangClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from GroupSang where Nom_GroupSang!='aucun';";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new GroupSangClass
                        {
                            IdGroupSang = (int)reader[0],
                            NonmGroupSang = (string)(reader[1])
                        });
                    }
                    reader.Close();
                }
                return list;
            }
        }

        //select gruop sang(combobox)
        public static List<KeyValuePair<int, String>> DisplayGroupSang()
        {
            List<KeyValuePair<int, String>> ListGroupSang = new List<KeyValuePair<int, string>>();
          
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select* from GroupSang";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        ListGroupSang.Add(new KeyValuePair<int, string>((int)reader[0], (String)reader[1]));
                    }
                    reader.Close();
                }
            }
            return ListGroupSang;
        }

        //select nom de group sang a partir de ID
        public static string NamofGroupSang(int id)
        {
            string NameGroupSAng = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from GroupSang where id_GroupSang=@id";
                    commande.Parameters.AddWithValue("@id", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        NameGroupSAng = reader[1].ToString();
                    }
                    reader.Close();
                }
            }
            return NameGroupSAng;
        }
    }
}
