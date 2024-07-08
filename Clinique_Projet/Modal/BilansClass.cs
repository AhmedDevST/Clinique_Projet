using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
   public class BilansClass
    {
        public string Result_Analyse { get; set; }
        public int ConsultID  { get; set; }
        public int Analyse_Bilan { get; set; }
        public DateTime DateBilan { get; set; } 
        public BilansClass() { }

        public BilansClass(int consult, int analyse, string res)
        {
            ConsultID = consult;
            Analyse_Bilan = analyse;
            Result_Analyse = res;
        }
        public BilansClass(int consult,int analyse,string res,DateTime date)
        {
            ConsultID = consult;
            Analyse_Bilan = analyse;
            DateBilan = date;
            Result_Analyse = res;
        }

        // add bilan
        public bool AddBilan()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                   
                        string sql = "insert into Bilans(Consult_id,Analyse_id,Result_Analyse,Date_Bilan)" +
                            " values (@consultId,@AnalyseId,@result_bilans,@date);";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", ConsultID);
                        cmd.Parameters.AddWithValue("@AnalyseId", Analyse_Bilan);
                        cmd.Parameters.AddWithValue("@result_bilans", Result_Analyse);
                        cmd.Parameters.AddWithValue("@date", DateBilan);
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

        //update bilan
        public bool UpdateBilans()
        {
            try { 
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Bilans " +
                            " set Result_Analyse=@result_bilans " +
                            " where Consult_id=@consultId and Analyse_id=@AnalyseId ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", ConsultID);
                        cmd.Parameters.AddWithValue("@AnalyseId", Analyse_Bilan);
                        cmd.Parameters.AddWithValue("@result_bilans", Result_Analyse);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true; ;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //display les analyse effectue
        public static ObservableCollection<int> Display_Analyse_effetue(int idc)
        {
            ObservableCollection<int> p = new ObservableCollection<int>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select Analyse_id from Bilans where Consult_id=@Consult_id;";
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
     

        // delete bilans
        public static bool DeleteBilans(int idconsult, int idanalyse)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Bilans " +
                            " where Consult_id=@consultId and Analyse_id=@AnalyseId;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@consultId", idconsult);
                        cmd.Parameters.AddWithValue("@AnalyseId", idanalyse);
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

        //return nombre des analyse dans une consultation
        public static int NBr_Bilans_INConsultation(int idConsult)
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Consult_id) from Bilans where Consult_id=@id";
                    commande.Parameters.AddWithValue("@id", idConsult);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb =Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
            }
            return nb;
        }
    }
}
