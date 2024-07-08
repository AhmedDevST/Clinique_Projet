using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace Clinique_Projet.Modal
{
    public class Categorie_Medicament
    {

        public int ID_CatMedicament { get; set; }
        public string Nom_CatMedicament { get; set; }

        public Categorie_Medicament() { }
        public Categorie_Medicament(int id, string nomType)
        {
            this.ID_CatMedicament = id;
            this.Nom_CatMedicament = nomType;
        }
        public Categorie_Medicament(string nomType)
        {
            this.Nom_CatMedicament = nomType;
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
                        commande.CommandText = "SELECT MAX(id_CatMedicament) FROM Categorie_Medicament;";
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
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

            return id;

        }
      
        // add cat mediCAt_cament
        public bool AddCat_Medicament()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Categorie_Medicament(id_CatMedicament,Nom_CatMedicament) " +
                            " values (@id_CatMedicament,@nom_Catmedicament) ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_CatMedicament", ID_CatMedicament);
                        cmd.Parameters.AddWithValue("@nom_Catmedicament", Nom_CatMedicament);
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

        // update mediCAt_cament
        public bool UpdateCat_Medicament()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Categorie_Medicament " +
                            " set Nom_CatMedicament=@nom_Typemedicament  " +
                            " where id_CatMedicament=@id_TypeMedicament;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_TypeMedicament", ID_CatMedicament);
                        cmd.Parameters.AddWithValue("@nom_Typemedicament", Nom_CatMedicament);
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

        // deleet cat medicament
        public static bool DeleteCat_medicament(int IdCat_Medicament)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Categorie_Medicament " +
                            " where id_CatMedicament=@IdCat_Medicament ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@IdCat_Medicament", IdCat_Medicament);
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

        //display ctegorie medicaments
        public static ObservableCollection<Categorie_Medicament> Display_CatMedicaments()
        {
            ObservableCollection<Categorie_Medicament> Cat_medca = new ObservableCollection<Categorie_Medicament>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select id_CatMedicament,Nom_CatMedicament from Categorie_Medicament order by id_CatMedicament desc;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        Cat_medca.Add(new Categorie_Medicament
                        {
                            ID_CatMedicament = (int)reader[0],
                            Nom_CatMedicament = (string)reader[1]
                        });
                    }
                    reader.Close();
                }
                return Cat_medca;
            }
        }
    }
}
