using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class GestionOrdonnance_Class
    {
        public Medicament_Class Medicament { get; set; }
        public Categorie_Medicament CatMedicament { get; set; }
        public Ordonnace_Class ordonnance { get; set; }
        public GestionOrdonnance_Class() { }

        //display  ordonnace
        public static ObservableCollection<GestionOrdonnance_Class> DisplayOrdonnance_Datagrid(int idc)
        {
            ObservableCollection<GestionOrdonnance_Class> p = new ObservableCollection<GestionOrdonnance_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select o.Medicament_id,m.Nom_Medicament ,c.Nom_CatMedicament,o.Posologie,o.Note_Supplimentaire,o.Date_Ordonnace, o.Quantite , o.Consult_id  " +
                        " from Ordonnance o ,Medicament m,Categorie_Medicament c " +
                        " where o.Consult_id=@idc and  o.Medicament_id=m.id_Medicament and m.CatMedicament_id=c.id_CatMedicament;";
                    commande.Parameters.AddWithValue("@idc", idc);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new GestionOrdonnance_Class
                        {
                            Medicament = new Medicament_Class { IdMedcament = (int)reader[0], NomMedcament = (string)reader[1] },
                            CatMedicament = new Categorie_Medicament {Nom_CatMedicament = (string)reader[2] },
                            ordonnance = new Ordonnace_Class { Posologie_Ordonnace = (string)reader[3], Note_Plus =reader[4].ToString(),DateOrdonnace = Convert.ToDateTime(reader[5]), Quantite = (int)reader[6] , Consult_Ordonnace = (int)reader[7] },
                        });
                    }
                    reader.Close();
                }
                return p;
            }
        }
    }
}
