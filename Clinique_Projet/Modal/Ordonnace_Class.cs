using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Ordonnace_Class
    {
        public string Posologie_Ordonnace { get; set; }
        public string Note_Plus { get; set; }
        public int Consult_Ordonnace { get; set; }
        public int Medicament_Ordonnace { get; set; }
        public DateTime DateOrdonnace { get; set; }
        public int Quantite { get; set; }
        public Ordonnace_Class() { }

        public Ordonnace_Class(string posologie,string note)
        {
            this.Posologie_Ordonnace = posologie;
            this.Note_Plus = note;
        }
        public Ordonnace_Class(int idc, int medicment, string posologie, string not, int Q)
        {
            this.Consult_Ordonnace = idc;
            this.Medicament_Ordonnace = medicment;
            this.Posologie_Ordonnace = posologie;
            this.Note_Plus = not;
            this.Quantite = Q;
        }

        public Ordonnace_Class(int idc, int medicament, DateTime date,string posologie, string not,int Q)
        {
            this.Consult_Ordonnace= idc;
            this.DateOrdonnace = date;
            this.Medicament_Ordonnace = medicament;
            this.Posologie_Ordonnace = posologie;
            this.Note_Plus = not;
            this.Quantite = Q;
        }

        // add ordonnance
        public bool AddOrdonnance()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = " insert into Ordonnance " +
                            "values(@consultId,@MedicamentId,@date,@posologie, @not,@Quantite)";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", Consult_Ordonnace);
                        cmd.Parameters.AddWithValue("@MedicamentId", Medicament_Ordonnace);
                        cmd.Parameters.AddWithValue("@posologie", Posologie_Ordonnace);
                        cmd.Parameters.AddWithValue("@date", DateOrdonnace);
                        cmd.Parameters.AddWithValue("@not", Note_Plus);
                        cmd.Parameters.AddWithValue("@Quantite", Quantite);
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
        // update ordonnance
        public bool UpdateOrdonnance()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = " update  Ordonnance " +
                            "  set Posologie=@posologie , Note_Supplimentaire=@not ,Quantite=@Quantite " +
                            " where Consult_id=@consultId and  Medicament_id=@MedicamentId ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", Consult_Ordonnace);
                        cmd.Parameters.AddWithValue("@MedicamentId", Medicament_Ordonnace);
                        cmd.Parameters.AddWithValue("@posologie", Posologie_Ordonnace);
                        cmd.Parameters.AddWithValue("@not", Note_Plus);
                        cmd.Parameters.AddWithValue("@Quantite", Quantite);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        // delete ordonnace
        public static bool DeleteOrdonnace(int idconsult, int idmedicament)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Ordonnance " +
                            " where Consult_id=@consultId and Medicament_id=@Medicament_id;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", idconsult);
                        cmd.Parameters.AddWithValue("@Medicament_id", idmedicament);
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

        //display les ordonnonce effectue
        public static ObservableCollection<int> Display_Ordonnonce_effetue(int idc)
        {
            ObservableCollection<int> p = new ObservableCollection<int>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Medicament_id from Ordonnance where Consult_id=@Consult_id;";
                    commande.Parameters.AddWithValue("@Consult_id", idc);
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

        // delete ordonnace
        public static bool DeleteOrdonnace_All(int idconsult)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Ordonnance " +
                            " where Consult_id=@consultId ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", idconsult);
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
     
        //return nombre des ordonnace dans une consultation
        public static int NBr_Ordonnnace_INConsultation(int idConsult)
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Consult_id) from Ordonnance where Consult_id=@id";
                    commande.Parameters.AddWithValue("@id", idConsult);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
            }
            return nb;
        }
    }
}
