using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class GestionBilan_class
    {
        public AnalyseClass Analyse { get; set; }
        public TypeAnalyse TypeAnalyse { get; set; }
        public BilansClass Bilans { get; set; }
        public GestionBilan_class() { }

        //display results of bilans
        public static ObservableCollection<GestionBilan_class> DisplayResultsBilans_Datagrid(int idc)
        {
            ObservableCollection<GestionBilan_class> p = new ObservableCollection<GestionBilan_class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select b.Analyse_id,a.Nom_Analyse ,b.Result_Analyse,t.Nom_TypeAN ,b.Date_Bilan,b.Consult_id " +
                        "from Bilans b ,Analyse a,Type_Analyse t " +
                        "where b.Consult_id=@idc and  b.Analyse_id=a.id_Analyse and a.Type_Analyse_id=t.id_TypeAN;";
                    commande.Parameters.AddWithValue("@idc", idc);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new GestionBilan_class
                        {
                            Analyse = new AnalyseClass { IdAnalyse = (int)reader[0], NomAnalyse = (string)reader[1] },
                            TypeAnalyse = new TypeAnalyse { Nom_TypeBilan = (string)reader[3] },
                            Bilans = new BilansClass { Result_Analyse = (string)reader[2], DateBilan = Convert.ToDateTime(reader[4]), ConsultID = (int)reader[5] },
                           
                        });
                    }
                    reader.Close();
                }
                return p;
            }
        }

        //display ANALYSE  d une consultation
        public static ObservableCollection<AnalyseClass> Display_analyse_consult(int idc)
        {
            ObservableCollection<AnalyseClass> p = new ObservableCollection<AnalyseClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select a.id_Analyse,a.Nom_Analyse " +
                        " from Bilans b ,Analyse a " +
                        " where b.Consult_id=@idc and  b.Analyse_id=a.id_Analyse ;";
                    commande.Parameters.AddWithValue("@idc", idc);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new AnalyseClass
                        {
                           IdAnalyse = (int)reader[0], NomAnalyse = (string)reader[1] 
                        });
                    }
                    reader.Close();
                }
                return p;
            }
        }


    }
}
