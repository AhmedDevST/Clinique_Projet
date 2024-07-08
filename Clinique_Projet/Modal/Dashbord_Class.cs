using Clinique_Projet.connectionDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Dashbord_Class
    {
        public int Number_Days { get; set; }
        public Dashbord_Class() { }

        //nombre de patient
        public static int Count_Patient()
        {
           int nb=0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
                return nb;
            }
        }

        //nombre de consultation
        public static int Count_Consultation()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_Consult) from Consultation;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
                return nb;
            }
        }

        //nombre de consultation
        public static int Count_RendezVous()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id) from RendezVous;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
                return nb;
            }
        }

        //nombre de homme
        public static int Count_Men()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient where Sex='H';";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb= (int)reader[0];
                     }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de femme
        public static int Count_Woman()
            {
                int nb = 0;
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var commande = new SqlCommand())
                    {
                        commande.Connection = con;
                        commande.CommandText = "select count(id_patient) from Patient where Sex='F';";
                        var reader = commande.ExecuteReader();
                        while (reader.Read())
                        {
                            nb = (int)reader[0];
                        }
                        reader.Close();
                    }
                }
                return nb;
            }
       
        //list  patient par categorie
        public static List<KeyValuePair<int, string>> Categorie_Patient()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
         if(Count_Categorie1_Lower_18()!=0) list.Add(new KeyValuePair<int, string>(Count_Categorie1_Lower_18(), "Moins de 18 ans"));
         if(Count_Categorie_Between18_40()!=0)  list.Add(new KeyValuePair<int, string>(Count_Categorie_Between18_40(), "Entre 18 ans et 40 ans"));
         if(Count_Categorie_Between40_70()!=0)   list.Add(new KeyValuePair<int, string>(Count_Categorie_Between40_70(), "Entre 40 ans et 70 ans"));
         if(Count_Categorie_Upeer_70()!=0)  list.Add(new KeyValuePair<int, string>(Count_Categorie_Upeer_70(), "Plus de 70 ans"));

            return list;
        }

        //list  patient par SEX
        public static List<KeyValuePair<int, string>> Categorie_Sex()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            list.Add(new KeyValuePair<int, string>(Count_Men(), "Homme"));
            list.Add(new KeyValuePair<int, string>(Count_Woman(), "Femme"));
           
            return list;
        }

        //nombre de categirie >18ans
        public static int Count_Categorie1_Lower_18()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient where Age_patient < 18;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de categoriie  >18 <40
        public static int Count_Categorie_Between18_40()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient where Age_patient >=18 and Age_patient<40;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de categoriie  >40 <70
        public static int Count_Categorie_Between40_70()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient where Age_patient >= 40 and Age_patient < 70 ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de categoriie  <70
        public static int Count_Categorie_Upeer_70()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient) from Patient where Age_patient > 70;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //list nombre consultaion par date
         public static List<KeyValuePair<int, DateTime>> Count_Consultation_ByDate(DateTime start_date,DateTime end_date)
        {
            List<KeyValuePair<int, DateTime>> listp = new List<KeyValuePair<int, DateTime>>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_Consult),Date_Consult " +
                                            "from Consultation " +
                                            " where Date_Consult between @depart and @fin" +
                                            " group by Date_Consult;";
                    commande.Parameters.AddWithValue("@depart", start_date);
                    commande.Parameters.AddWithValue("@fin", end_date);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        listp.Add(new KeyValuePair<int, DateTime>((int)reader[0],
                                  Convert.ToDateTime(reader[1]) ));
                    }
                    reader.Close();
                }
            }

            return listp;
        }

        //list nombre consultaion par date
        public static List<KeyValuePair<int, DateTime>> Count_RDV_ByDate(DateTime start_date, DateTime end_date)
        {
            List<KeyValuePair<int, DateTime>> listp = new List<KeyValuePair<int, DateTime>>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id),date " +
                                            "from RendezVous " +
                                            " where date between @depart and @fin" +
                                            " group by date;";
                    commande.Parameters.AddWithValue("@depart", start_date);
                    commande.Parameters.AddWithValue("@fin", end_date);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        listp.Add(new KeyValuePair<int, DateTime>((int)reader[0],
                                  Convert.ToDateTime(reader[1])));
                    }
                    reader.Close();
                }
            }

            return listp;
        }


        //list nombre des patient de chaque categorie de assuranec 
        public static List<KeyValuePair<int, string>> Categorie_Assurance()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(p.id_patient),a.Nom_Assurance  " +
                                             " from Assurance a,Patient p " +
                                            " where p.Assurance_id=a.id_Assurance " +
                                            " group by a.Nom_Assurance;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new KeyValuePair<int, string>((int)reader[0],
                                  (string)reader[1]));
                    }
                    reader.Close();
                }
            }

            return list;
        }

        //list nombre des patient de chaque categorie de groupsang 
        public static List<KeyValuePair<int, string>> Categorie_GroupSang()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(id_patient),Nom_GroupSang " +
                                            "from GroupSang,Patient " +
                                            " where GroupSang_id=id_GroupSang" +
                                            " group by Nom_GroupSang;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new KeyValuePair<int, string>((int)reader[0],
                                  (string)reader[1]));
                    }
                    reader.Close();
                }
            }

            return list;
        }

        //** certificate **//

        //list  certificate par categorie
        public static List<KeyValuePair<int, string>> Categorie_Certificate()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            list.Add(new KeyValuePair<int, string>(Count_Certificate_Medical(), "CERTIFICAT MEDICAL"));
            list.Add(new KeyValuePair<int, string>(Count_aptitude_physique(), "APTITUDE PHYSIQUE"));
            list.Add(new KeyValuePair<int, string>(Count_certificate_mariage(), "شهادة البلوغ للزواج"));
            list.Add(new KeyValuePair<int, string>(Count_certificate_aptitude_mariage(), " إبرام عقد الزواج"));

            return list;
        }

        //nombre de certificate 
        public static int Count_Certificates()
        {
            int nb = 0;
            nb = Count_Certificate_Medical() +
                Count_aptitude_physique() +
                Count_certificate_mariage()
                + Count_certificate_aptitude_mariage();
            return nb;
        }


        //nombre de certificate medical
        public static int Count_Certificate_Medical()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id) from certificat ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de certificate aptitude physique
        public static int Count_aptitude_physique()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id) from apt_physique ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }

        //nombre de certificate mariage 
        public static int Count_certificate_mariage()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id) from mariage where nom_w != '0';";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }
        //nombre de certificate aptitude  mariage 
        public static int Count_certificate_aptitude_mariage()
        {
            int nb = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select count(Id) from mariage where nom_w = '0';";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
            }
            return nb;
        }


    }
}
