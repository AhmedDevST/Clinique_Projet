using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class TypeAntecedent
    {
        public int ID_TypeANteced { get; set; }
        public string Nom_TypeANteced { get; set; }

        public TypeAntecedent() { }
        public TypeAntecedent(int id,string nom) 
        { 
            this.ID_TypeANteced = id;
            this.Nom_TypeANteced = nom;
        }

        // add antecdent
        public bool Add_TypeAntecdent()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Type_Antecedent( Nom_TypeAtecd ) " +
                            " values (@Nom_TypeAtecd ) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Nom_TypeAtecd", Nom_TypeANteced);
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
        // update TypeAntecdent
        public bool Update_TypeAntecdent()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Type_Antecedent " +
                            " set Nom_TypeAtecd=@Nom_TypeAtecd " +
                            " where id_TypeAtecd=@idTYpe;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idTYpe", ID_TypeANteced);
                        cmd.Parameters.AddWithValue("@Nom_TypeAtecd", Nom_TypeANteced);
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
        // delete TypeAntecdent
        public static bool Delete_TypeAntecdent(int IdType)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Type_Antecedent " +
                            " where id_TypeAtecd=@idTYpe;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idTYpe", IdType);
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
        //display type antecedent 
        public static ObservableCollection<TypeAntecedent> DisplayTypeAnteced()
        {
            ObservableCollection<TypeAntecedent> list_anteced = new ObservableCollection<TypeAntecedent>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select * from Type_Antecedent;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list_anteced.Add(new TypeAntecedent
                        {
                            ID_TypeANteced = (int)reader[0],
                            Nom_TypeANteced =(string)(reader[1])
                        });
                    }
                    reader.Close();
                }
                return list_anteced;
            }
        }
      
        //select nom de type de antecdents
        public static string SelectNAme_TypeAnteced(int idt)
        {
            string TypeAntecd = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select * from Type_Antecedent where id_TypeAtecd=@idt;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idt", idt);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        TypeAntecd = new string((string)reader[1]);
                    }
                    reader.Close();
                }
                return TypeAntecd;
            }
        }
    }
}
