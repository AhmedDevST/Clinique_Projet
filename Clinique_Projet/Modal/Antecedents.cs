using Clinique_Projet.connectionDb;
using System;
using System.Data.SqlClient;

namespace Clinique_Projet.Modal
{
    public class Antecedents
    {
        public int IdPatient { get; set; }
        public int IDType_Anteced { get; set; }
        public DateTime Date_Anteced { get; set; }
        public string Descrip_Anteced { get; set; }

        public Antecedents() { }
        public Antecedents(int typeAn,DateTime datea,string des)
        { 
            this.Descrip_Anteced = des;
            this.IDType_Anteced=typeAn;
            this.Date_Anteced = datea; 
        }
        public Antecedents(int idp, int typeAn, string des)
        {
            this.IdPatient = idp;
            this.Descrip_Anteced = des;
            this.IDType_Anteced = typeAn;
        }
        public Antecedents(int idp,int typeAn, DateTime datea, string des)
        {
            this.IdPatient = idp;
            this.Descrip_Anteced = des;
            this.IDType_Anteced = typeAn;
            this.Date_Anteced = datea;
        }

        // add Antecedents
        public bool AddAntecedent()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "insert into Antecedents(patient_id,TypeAtecd_id,Descrip_Antecedent,Date_Anteced) " +
                            " values (@patientId,@TypeAntecedId,@description,@date);";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@patientId", IdPatient);
                        cmd.Parameters.AddWithValue("@TypeAntecedId", IDType_Anteced);
                        cmd.Parameters.AddWithValue("@description", Descrip_Anteced);
                        cmd.Parameters.AddWithValue("@date", Date_Anteced);
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

        // update Antecedents
        public bool UpdateAntecedent()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "update  Antecedents " +
                            " set Descrip_Antecedent=@description " +
                            " where patient_id=@patientId and TypeAtecd_id=@TypeAntecedId ;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@patientId", IdPatient);
                        cmd.Parameters.AddWithValue("@TypeAntecedId", IDType_Anteced);
                        cmd.Parameters.AddWithValue("@description", Descrip_Anteced);
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

        // delete Antecedents
        public static bool DeleteAntecedent(int patient,int typeanteced)
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        string sql = "delete  Antecedents " +
                            " where patient_id=@patientId and TypeAtecd_id=@TypeAntecedId;";
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@patientId", patient);
                        cmd.Parameters.AddWithValue("@TypeAntecedId", typeanteced);
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

        //select antecedent par  patient et le type de antecendet
        public static Antecedents SelectAnteced(int idp,int typeAntecd)
        {
            Antecedents Antecd = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select a.Date_Anteced, a.Descrip_Antecedent,a.TypeAtecd_id " +
                             "from Antecedents a where a.patient_id=@idPatient and a.TypeAtecd_id=@idType;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idPatient", idp);
                    commande.Parameters.AddWithValue("@idType", typeAntecd);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        Antecd = new Antecedents(
                             (int)reader[2],
                            (DateTime)reader[0],
                            (string)reader[1]
                                           );
                    }
                    reader.Close();
                }
                return Antecd;
            }
        }

        //tester si patient a des antecedent ou non
        public static bool Existance_Antecedent_Patient(int idp)
        {
            bool existe =false;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "select * from Antecedents a where a.patient_id=@idPatient;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@idPatient", idp);
                    var reader = commande.ExecuteReader();
                    if (reader.Read()) existe=true;
                    reader.Close();
                }
                return existe;
            }
        }
    }
}
