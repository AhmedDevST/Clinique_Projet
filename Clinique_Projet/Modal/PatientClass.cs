using Clinique_Projet.connectionDb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class PatientClass
    {
        public int IDPatient { get; set; }
        public string NomPatient { get; set; }
        public string PrenomPatient { get; set; }
        public string CINPatient { get; set; }
        public DateTime DateNaissancePatient { get; set; }
        public char SexPatient { get; set; }
        public string LieuNaissancePatient { get; set; }
        public int AgePatient { get; set; }
        public string PhonePatient { get; set; }
        public string EmailPatient { get; set; }
        public string AddressPatient { get; set; }
        public string ProfessPatient { get; set; }
        public DateTime DateSavePatient { get; set; }
        public string AssurancePatient { get; set; }
        public string GroupSangPatient { get; set; }
        public string EtatCivilPatient { get; set; }

        public PatientClass() { }

        public PatientClass(int id,string nom,string prenom,DateTime datNAiss,string cin,char sex,
            DateTime dateSave,int age,string lieuDate,string phone,string email,string address,
            string proff,string assurance,string sang,string etatCivil) 
        {
            this.IDPatient = id;
            this.NomPatient = nom;
            this.PrenomPatient= prenom;
            this.CINPatient = cin;
            this.SexPatient = sex;
            this.DateSavePatient = dateSave;
            this.DateNaissancePatient = datNAiss;
            this.AgePatient = age;
            this.PhonePatient = phone;
            this.EmailPatient = email;
            this.AddressPatient = address;
            this.AssurancePatient = assurance;
            this.GroupSangPatient = sang;
            this.EtatCivilPatient= etatCivil;
            this.ProfessPatient= proff;
            this.LieuNaissancePatient= lieuDate;
        }


        public PatientClass(int id, string nom, string prenom, char sex,
                            string cin, int age,string phone,string assurance)
        {
            this.IDPatient = id;
            this.NomPatient = nom;
            this.PrenomPatient = prenom;
            this.AssurancePatient = assurance;
            this.CINPatient= cin;
            this.SexPatient = sex;
            this.AgePatient = age;
            this.PhonePatient = phone;
        }


        //get Id of last row
        public static int GetLast_Id()
        {
            int idpat = 0;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = " select MAX(id_patient) from Patient;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        idpat = Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                return idpat;
            }
        }
        // add patient
        public bool AddPatient()
        {
            try 
            { 
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Patient(Nom_patient,Prenom_patient,CIN_patient,DateNaissance,Sex," +
                            "LieuNaissance,Phone_patient,Email_patient,Address_patient,Profession_patient," +
                            "Age_patient,Assurance_id,GroupSang_id,EtatCivil)" +
                            "values (@nom,@prenom,@cin,@dateNaiss,@sex,@lieuNAiss,@phone,@email,@address," +
                            "@profess,@age,@assurance,@sang,@civil);";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@nom",NomPatient);
                        cmd.Parameters.AddWithValue("@prenom", PrenomPatient);
                        cmd.Parameters.AddWithValue("@cin", CINPatient);
                        cmd.Parameters.AddWithValue("@dateNaiss", DateNaissancePatient);
                        cmd.Parameters.AddWithValue("@sex", SexPatient);
                        cmd.Parameters.AddWithValue("@lieuNAiss", LieuNaissancePatient);
                        cmd.Parameters.AddWithValue("@phone", PhonePatient);
                        cmd.Parameters.AddWithValue("@email", EmailPatient);
                        cmd.Parameters.AddWithValue("@address", AddressPatient);
                        cmd.Parameters.AddWithValue("@profess", ProfessPatient);
                        cmd.Parameters.AddWithValue("@age", AgePatient);
                        cmd.Parameters.AddWithValue("@assurance", Convert.ToInt32(AssurancePatient));
                        cmd.Parameters.AddWithValue("@sang", Convert.ToInt32(GroupSangPatient));
                        cmd.Parameters.AddWithValue("@civil", EtatCivilPatient);
               
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
       
        //edit patient
        public bool UpdatePatient()
        {
            try 
            { 
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update Patient set Nom_patient=@nom,Prenom_patient=@prenom ,CIN_patient=@cin" +
                            " , DateNaissance=@dateNaiss ,Sex=@sex,LieuNaissance=@lieuNAiss,Phone_patient=@phone" +
                            " , Email_patient=@email , Address_patient=@address , Profession_patient=@profess" +
                            " , Age_patient=@age , Assurance_id=@assurance , GroupSang_id=@sang , EtatCivil=@civil " +
                            "  where id_patient=@idpatient ;";
                       
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idpatient", IDPatient);
                        cmd.Parameters.AddWithValue("@nom", NomPatient);
                        cmd.Parameters.AddWithValue("@prenom", PrenomPatient);
                        cmd.Parameters.AddWithValue("@cin", CINPatient);
                        cmd.Parameters.AddWithValue("@dateNaiss", DateNaissancePatient);
                        cmd.Parameters.AddWithValue("@sex", SexPatient);
                        cmd.Parameters.AddWithValue("@lieuNAiss", LieuNaissancePatient);
                        cmd.Parameters.AddWithValue("@phone", PhonePatient);
                        cmd.Parameters.AddWithValue("@email", EmailPatient);
                        cmd.Parameters.AddWithValue("@address", AddressPatient);
                        cmd.Parameters.AddWithValue("@profess", ProfessPatient);
                        cmd.Parameters.AddWithValue("@age", AgePatient);
                        cmd.Parameters.AddWithValue("@assurance", Convert.ToInt32(AssurancePatient));
                        cmd.Parameters.AddWithValue("@sang", Convert.ToInt32(GroupSangPatient));
                        cmd.Parameters.AddWithValue("@civil", EtatCivilPatient);

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

        //delete patient

        public static bool DeletePatient(int id)
        {
            try
            {
                string sql = "DELETE FROM Patient WHERE id_patient=@idPatient ; ";
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@idPatient", id);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        //select patient
        public static PatientClass SelectPatient(int id)
        {
            PatientClass patient=null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                 string sql = "select id_patient,Nom_patient,Prenom_patient,CIN_patient,DateNaissance,Sex,LieuNaissance,Phone_patient,Email_patient,Address_patient,Profession_patient,Assurance_id,GroupSang_id,EtatCivil,Age_patient,DateSave_patient from Patient where id_patient=@idPatient;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idPatient", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        patient = new PatientClass(

                             (int)reader[0], (string)reader[1], (string)reader[2], (DateTime)reader[4],
                             (string)reader[3], Convert.ToChar(reader[5]), (DateTime)reader[15], (int)reader[14],
                            (string)reader[6], (string)reader[7], (string)reader[8], (string)reader[9],
                            (string)reader[10],reader[11].ToString(), reader[12].ToString(), (string)reader[13]
                           
                                                   );
                    }
                    reader.Close();
                }
                return patient;
            }
        }
        
        //display patients
        public static ObservableCollection<PatientClass> DisplayPatinet_Datagrid()
        {
            ObservableCollection<PatientClass> p = new ObservableCollection<PatientClass>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select id_patient,Nom_patient,Prenom_patient,Sex,CIN_patient,Age_patient,Phone_patient,a.Nom_Assurance from Patient p,Assurance a \r\nwhere p.Assurance_id=a.id_Assurance;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        p.Add(new PatientClass { IDPatient = (int)reader[0], NomPatient = (string)reader[1],
                            PrenomPatient = (string)reader[2], SexPatient = Convert.ToChar(reader[3]), CINPatient = (string)reader[4],
                        AgePatient = (int)reader[5], PhonePatient = (string)reader[6], AssurancePatient = (string)reader[7] });
                    }
                    reader.Close();
                }
                return p;
            }
        }

        //select nom prenom de patient
        public static string SelectNomPatient(int id)
        {
            string patient = string.Empty;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select id_patient,Nom_patient,Prenom_patient " +
                          " from Patient where id_patient=@idPatient;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idPatient", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        patient = reader[1].ToString() + " " + reader[2].ToString();
                    }
                    reader.Close();
                }
                con.Close();
                return patient;
            }
        }

        //llist of patient CIN 
        public static List<string> DisplayCIN_Patient()
        {
           List<string> CINs = new List<string>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select CIN_patient from Patient ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        CINs.Add(new string(reader[0].ToString()));
                    }
                    reader.Close();
                }
                return CINs;
            }
        }

        public static List<KeyValuePair<int, string>> ListPatient()
        {
            List<KeyValuePair<int, string>> listp = new List<KeyValuePair<int, string>>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = "select id_patient,Nom_patient,Prenom_patient," +
                                         " CIN_patient from Patient;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        listp.Add(new KeyValuePair<int, string>((int)reader[0],
                            reader[1] + " ; " + reader[2] + " ; " + reader[3].ToString()));
                    }
                    reader.Close();
                }
            }
            return listp;
        }
    }
}
