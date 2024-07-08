using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Clinique_Projet.Modal
{
    public class Notification_class
    {
        public int Id_Notification { get; set; }
        public int Id_user { get; set; }
        public int Id_operation { get; set; }
        public string Type_operation { get; set; }
        public DateTime Date_notification { get; set; }
        public string Description_notification { get; set; }

        public Notification_class(int id_Notification, int id_user, int id_operation, string type_operation, DateTime date_notification, string description_notification)
        {
            Id_Notification = id_Notification;
            Id_user = id_user;
            Id_operation = id_operation;
            Type_operation = type_operation;
            Date_notification = date_notification;
            Description_notification = description_notification;
        }
        public Notification_class() { }

        public void Add_Notification()
        {
            switch (this.Type_operation)
            {
                case "pat":add_notification_patient(); break;
                case "cons":add_notification_consultation(); break;
                case "rdv":add_notification_RendezVous(); break;
            }
        }
        public void add_notification_patient()
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    string sql = "INSERT INTO Notification_patient (patient_id, user_id,Date_notification,description_notification) " +
                        "VALUES (@patient_id, @user_id,@Date_notification,@description_notification);";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@patient_id", Id_operation);
                    cmd.Parameters.AddWithValue("@user_id", Id_user);
                    cmd.Parameters.AddWithValue("@Date_notification", Date_notification);
                    cmd.Parameters.AddWithValue("@description_notification", Description_notification);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public void add_notification_consultation()
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    string sql = "INSERT INTO Notification_Consultation (consult_id, user_id,Date_notification,description_notification) " +
                        "VALUES (@consult_id, @user_id,@Date_notification,@description_notification);";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@consult_id", Id_operation);
                    cmd.Parameters.AddWithValue("@user_id", Id_user);
                    cmd.Parameters.AddWithValue("@Date_notification", Date_notification);
                    cmd.Parameters.AddWithValue("@description_notification", Description_notification);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public void add_notification_RendezVous()
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    string sql = "INSERT INTO Notification_RendezVous (rdv_id, user_id,Date_notification,description_notification) " +
                        "VALUES (@rdv_id, @user_id,@Date_notification,@description_notification);";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@rdv_id", Id_operation);
                    cmd.Parameters.AddWithValue("@user_id", Id_user);
                    cmd.Parameters.AddWithValue("@Date_notification", Date_notification);
                    cmd.Parameters.AddWithValue("@description_notification", Description_notification);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static ObservableCollection<Notification_class> Display_all_notification()
        {
            ObservableCollection<Notification_class> List = new ObservableCollection<Notification_class>(
               Display_notification_by_Categorie("pat").
               Concat(Display_notification_by_Categorie("cons")).
               Concat(Display_notification_by_Categorie("rdv"))
                );
            return List;
        }
        public static ObservableCollection<Notification_class> Display_notification_by_Categorie(string type)
        {
            ObservableCollection<Notification_class> List = new ObservableCollection<Notification_class>();
            string sql = string.Empty;
            switch (type)
            {
                case "pat":
                    sql = "select p.id_notification,p.patient_id,p.Date_notification,p.description_notification,u.Nom_user,u.Prenom_user,u.Role_user,u.IsAdmin " +
                        " from Notification_patient p,Utilisateur u " +
                        " where p.user_id=u.id_user " +
                        " order by p.id_notification DESC;";
                    break;
                case "cons":
                    sql = "select p.id_notification,p.consult_id,p.Date_notification,p.description_notification,u.Nom_user,u.Prenom_user,u.Role_user,u.IsAdmin " +
                        " from Notification_Consultation p,Utilisateur u " +
                        " where p.user_id=u.id_user " +
                        "  order by p.id_notification DESC;";
                    break;
                case "rdv":
                    sql = "select p.id_notification,p.rdv_id,p.Date_notification,p.description_notification,u.Nom_user,u.Prenom_user,u.Role_user,u.IsAdmin " +
                        " from Notification_RendezVous p,Utilisateur u" +
                        " where p.user_id=u.id_user " +
                        " order by p.id_notification DESC;";
                    break;
            }
            if (type != string.Empty)
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var commande = new SqlCommand())
                    {
                        commande.Connection = con;
                        commande.CommandText = sql;
                        var reader = commande.ExecuteReader();
                        while (reader.Read())
                        {
                            //selection le role
                            string role_user = string.Empty;
                            if (Convert.ToInt32(reader[7]) == 1)
                            {
                                role_user = "Administrateur";
                            }
                            else
                            {
                                switch (Convert.ToInt32(reader[6]))
                                {
                                    case 1: role_user = "Medcine"; break;
                                    case 2: role_user = "secretaire"; break;
                                }
                            }

                            List.Add(new Notification_class
                            {
                                Id_Notification = (int)reader[0],
                                Id_operation = (int)reader[1],
                                Type_operation = type,
                                Date_notification = Convert.ToDateTime(reader[2]),
                                Description_notification = (string)reader[4] + " " + (string)reader[5] + "( " + role_user + " ) " + " a  " + (string)reader[3] + " au " + Convert.ToDateTime(reader[2]).ToShortDateString()
                            });
                        }
                        reader.Close();
                    }
                    return List;
                }
            }
            return List;
        }
       
    }
}
