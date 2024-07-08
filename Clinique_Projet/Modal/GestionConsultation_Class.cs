using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class GestionConsultation_Class
    {
        public PatientClass Patient { get; set; }
        public ConsultationClass consultation { get; set; }
        public GestionConsultation_Class() { }

        //display all consultation
        public static ObservableCollection<GestionConsultation_Class> DisplayAll_Consultation()
        {
            ObservableCollection<GestionConsultation_Class> p = new ObservableCollection<GestionConsultation_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                   
                    commande.Connection = con;
                    commande.CommandText = "select p.id_patient,p.Nom_patient,p.Prenom_patient,c.id_Consult,c.Motif_Consult,c.Date_Consult,c.Terminer_Consult " +
                                            " from Consultation c , Patient p" +
                                            " where c.Patient_id=p.id_patient" +
                                            " Order by c.Date_Consult DESC;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new GestionConsultation_Class
                        {
                            Patient = new PatientClass()
                            {
                                IDPatient = (int)reader[0],
                                NomPatient = reader[1].ToString(),
                                PrenomPatient = reader[2].ToString()
                            },
                            consultation = new ConsultationClass()
                            {
                                IdConsult = (int)reader[3],
                                MotifConsult = reader[4].ToString(),
                                DateConsult = Convert.ToDateTime(reader[5]),
                                Terminer_consult = (int)reader[6]
                            },
                        }) ; 
                    }
                    reader.Close();
                }
                return p;
            }
        }

    }
}
