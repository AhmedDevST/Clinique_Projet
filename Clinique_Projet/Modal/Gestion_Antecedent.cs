using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Gestion_Antecedent
    {
        public Antecedents anteced { get; set; }
        public TypeAntecedent type_anteced { get; set; }

        public Gestion_Antecedent() { }

        //display  antecedent  de patient
        public static ObservableCollection<Gestion_Antecedent> DisplayAnteced_OfPatient(int idp)
        {
            ObservableCollection<Gestion_Antecedent> list_anteced = new ObservableCollection<Gestion_Antecedent>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select a.TypeAtecd_id,a.Date_Anteced,a.Descrip_Antecedent,t.Nom_TypeAtecd " +
                             "from Antecedents a,Type_Antecedent t " +
                             " where a.patient_id=@idPatient and a.TypeAtecd_id=t.id_TypeAtecd;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idPatient", idp);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list_anteced.Add(new Gestion_Antecedent
                        {
                           anteced=new Antecedents { IDType_Anteced = Convert.ToInt32(reader[0]),
                                                 Date_Anteced = Convert.ToDateTime(reader[1]),
                                                 Descrip_Anteced = reader[2].ToString()  },
                           type_anteced=new TypeAntecedent { Nom_TypeANteced = reader[3].ToString() },        
                        });
                    }
                    reader.Close();
                }
                return list_anteced;
            }
        }

        // add all antecedents
        public static void Add_AllAntecedents(int idPatient, ObservableCollection<Gestion_Antecedent> list)
        {
            foreach (var item in list)
            {
                Antecedents Anteced = new Antecedents(idPatient, item.anteced.IDType_Anteced,
                                     item.anteced.Date_Anteced, item.anteced.Descrip_Anteced);
                Anteced.AddAntecedent();
            }
        }
    }
}
