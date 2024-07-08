using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Prochaine_RDV_class
    {
        public int  Prochaine_RDV_id {get; set;}
        public int consultation_id { get; set;} 

        public Prochaine_RDV_class(int id_rdv, int consultation_id)
        {
            this.consultation_id = consultation_id;
            this.Prochaine_RDV_id = id_rdv;
        }
        public  void ADD_Prochaine_rdv()
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {

                        commande.Connection = con;
                        commande.CommandText = "INSERT INTO Prochaine_RDV(Consult_id,RDV_id) Values(@Consult_id,@RDV_id)";
                        commande.Parameters.AddWithValue("@Consult_id", consultation_id);
                        commande.Parameters.AddWithValue("@RDV_id", Prochaine_RDV_id);
                        commande.ExecuteNonQuery();
                        con.Close();
                }
            }
        }

        //seletion id le prochaine rendez vous
        public static int Selection_PRDV(DateTime? date, string time, string type, int id_p, string description)
        {
            int rendezVous = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "SELECT Id " +
                              " FROM RendezVous " +
                              " where id_patient=@id_patient and " +
                              " date=@date and " +
                              " heure=@heure and " +
                              " description=@description and " +
                              " type=@type ;";
                    commande.Parameters.AddWithValue("@id_patient", id_p);
                    commande.Parameters.AddWithValue("@date", date);
                    commande.Parameters.AddWithValue("@heure", time);
                    commande.Parameters.AddWithValue("@description", description);
                    commande.Parameters.AddWithValue("@type", type);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        rendezVous = (int)reader[0];
                    }
                    reader.Close();
                }


            }
            return rendezVous;
        }

        //seletion les prochaine rendez vous d une consultation
        public static ObservableCollection<RendezVous> Display_PRDV(int id_consult)
        {
            ObservableCollection<RendezVous> list = new ObservableCollection<RendezVous>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select rdv.Id,rdv.date,rdv.heure,rdv.description,rdv.type " +
                        " from Prochaine_RDV prdv ,RendezVous rdv " +
                        " where prdv.Consult_id=@Consult_id and prdv.RDV_id=rdv.Id" +
                        " Order by date  ;";
                    commande.Parameters.AddWithValue("@Consult_id", id_consult);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new RendezVous
                        {
                            id = (int)reader[0],
                            date = (DateTime)reader[1],
                            heure = (TimeSpan)reader[2],
                            description = (string)reader[3],
                            type = (string)reader[4]
                        });
                    }
                    reader.Close();
                }
            }
            return list;
        }

        //nombre prochaine  les prochaine rendez vous d une consultation
        public static int Count_PRDV(int id_consult)
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(RDV_id)  " +
                        " from Prochaine_RDV  " +
                        " where Consult_id=@Consult_id  ;";
                    commande.Parameters.AddWithValue("@Consult_id", id_consult);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                       nb=  (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }
    }
}
