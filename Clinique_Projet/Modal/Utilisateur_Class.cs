using Clinique_Projet.connectionDb;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
namespace Clinique_Projet.Modal
{
    public class Utilisateur_Class
    {
        public int id_user { get; set; }
        public string nom_user { get; set; }
        public string prenom_user { get; set; }
        public string CIN_user { get; set; }
        public string phone_user { get; set; }
        public string email_user { get; set; }
        public string login_user { get; set; }
        public string password_user { get; set; }
        public byte[] imageData { get; set; }
        public int Status_compte { get; set; }
        public string diplome_user { get; set; }
        public string IsConnect { get; set; }

        public int IsAdmin { get; set; }
        public string Role_user { get; set; }
        /*    
         *     1 :medcine,admin
         *     2: secreatire */

        public Utilisateur_Class() { }

        public Utilisateur_Class(int id, string role, int isadmin, string nom, string prenom)
        {

            this.id_user = id;
            this.Role_user = role;
            this.IsAdmin = isadmin;
            this.nom_user = nom;
            this.prenom_user = prenom;
        }

        public Utilisateur_Class(int id_user, string nom_user, string prenom_user, string cIN_user, string phone_user, string email_user, string imagePath, int status_compte, string diplome_user, string login_user, string password_user, string role_user)
        {
            this.id_user = id_user;
            this.nom_user = nom_user;
            this.prenom_user = prenom_user;
            CIN_user = cIN_user;
            this.phone_user = phone_user;
            this.email_user = email_user;
            this.login_user = login_user;
            this.password_user = password_user;
            Status_compte = status_compte;
            this.diplome_user = diplome_user;
            Role_user = role_user;
            if (!string.IsNullOrEmpty(imagePath))
            {
                this.imageData = File.ReadAllBytes(imagePath);
            }
            else imageData = null;
        }

        public Utilisateur_Class(int id_user, string nom_user, string prenom_user, string cIN_user, string phone_user, string email_user, string diplome_user, byte[] imageData, int status_compte, string login_user, string password_user, string role_user, int isadmin, string iscoonect)
        {
            this.id_user = id_user;
            this.nom_user = nom_user;
            this.prenom_user = prenom_user;
            CIN_user = cIN_user;
            this.phone_user = phone_user;
            this.email_user = email_user;
            this.login_user = login_user;
            this.password_user = password_user;
            Status_compte = status_compte;
            this.diplome_user = diplome_user;
            Role_user = role_user;
            this.imageData = imageData;
            this.IsAdmin = isadmin;
            this.IsConnect = iscoonect;
        }

        // add user
        public bool AddUser()
        {
            try 
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();

                    // ADD 
                    string sql = "insert into Utilisateur(Nom_user, Prenom_user, CIN_user, Phone_user, Email_user, " +
                       " Diplome_user,Status_Compte,Role_user,Login_user," +
                       " password_user)" +
                       " values (@Nom_user,@Prenom_user,@CIN_user,@Phone_user,@Email_user,@Diplome_user,@Status_Compte,@Role_user," +
                       " @Login_user,ENCRYPTBYPASSPHRASE('key_user',@password_user));";

                    if (imageData != null) sql = "insert into Utilisateur(Nom_user, Prenom_user, CIN_user, Phone_user, Email_user, " +
                       " Diplome_user,Photo_user,Status_Compte,Role_user,Login_user," +
                       " password_user)" +
                       " values (@Nom_user,@Prenom_user,@CIN_user,@Phone_user,@Email_user,@Diplome_user,@Photo_user,@Status_Compte,@Role_user," +
                       " @Login_user,ENCRYPTBYPASSPHRASE('key_user',@password_user));";

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@Nom_user", nom_user);
                        cmd.Parameters.AddWithValue("@Prenom_user", prenom_user);
                        cmd.Parameters.AddWithValue("@CIN_user", CIN_user);
                        cmd.Parameters.AddWithValue("@Phone_user", phone_user);
                        cmd.Parameters.AddWithValue("@Email_user", email_user);
                        cmd.Parameters.AddWithValue("@Diplome_user", diplome_user);
                        if (imageData != null) cmd.Parameters.AddWithValue("@Photo_user", imageData);
                        cmd.Parameters.AddWithValue("@Status_Compte", Status_compte);
                        cmd.Parameters.AddWithValue("@Role_user", Convert.ToInt32(Role_user));
                        cmd.Parameters.AddWithValue("@Login_user", login_user);
                        cmd.Parameters.AddWithValue("@password_user", password_user);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // update user
        public bool UpdateUser()
        {
            try
            {
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();

                    // update 
                    // pas de modifer image
                    string sql = "update  Utilisateur set Nom_user=@Nom_user,Prenom_user=@Prenom_user,CIN_user=@CIN_user," +
                        " Phone_user=@Phone_user,Email_user=@Email_user, " +
                       " Diplome_user=@Diplome_user,Status_Compte=@Status_Compte,Role_user=@Role_user," +
                       " Login_user= @Login_user, password_user=ENCRYPTBYPASSPHRASE('key_user',@password_user) " +
                       " where id_user=@id_user ;";

                    //modifer image
                    if (imageData != null)
                        sql = "update  Utilisateur set Nom_user=@Nom_user,Prenom_user=@Prenom_user,CIN_user=@CIN_user," +
                       " Phone_user=@Phone_user,Email_user=@Email_user, " +
                      " Diplome_user=@Diplome_user,Photo_user=@Photo_user,Status_Compte=@Status_Compte,Role_user=@Role_user," +
                      " Login_user= @Login_user, password_user=ENCRYPTBYPASSPHRASE('key_user',@password_user) " +
                      " where id_user=@id_user ;";

                    using (var cmd = new SqlCommand())
                    {

                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_user", id_user);
                        cmd.Parameters.AddWithValue("@Nom_user", nom_user);
                        cmd.Parameters.AddWithValue("@Prenom_user", prenom_user);
                        cmd.Parameters.AddWithValue("@CIN_user", CIN_user);
                        cmd.Parameters.AddWithValue("@Phone_user", phone_user);
                        cmd.Parameters.AddWithValue("@Email_user", email_user);
                        cmd.Parameters.AddWithValue("@Diplome_user", diplome_user);
                        if (imageData != null) cmd.Parameters.AddWithValue("@Photo_user", imageData);
                        cmd.Parameters.AddWithValue("@Status_Compte", Status_compte);
                        cmd.Parameters.AddWithValue("@Role_user", Convert.ToInt32(Role_user));
                        cmd.Parameters.AddWithValue("@Login_user", login_user);
                        cmd.Parameters.AddWithValue("@password_user", password_user);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //delete user
        public static bool DeleteUser(int id)
        {
            try
            {
                string sql = "DELETE FROM Utilisateur WHERE id_user=@id_user ; ";
                using (var con = ConnectDb.GetConnection())
                {
                    con.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id_user", id);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        // update STAUTS  user
        public static void Update_Status_User(int id_user, int status)
        {
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                string sql = "update  Utilisateur set IsConnect=@IsConnect " +
                         " where id_user=@id_user ;";

                using (var cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id_user", id_user);
                    cmd.Parameters.AddWithValue("@IsConnect", status);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        //select user
        public static Utilisateur_Class SelectUser(int id)
        {
            Utilisateur_Class user = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();

                string sql = "select id_user,Nom_user,Prenom_user,CIN_user,Phone_user,Email_user,Diplome_user " +
                    " ,Photo_user,Status_Compte,Login_user, CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('key_user',password_user))  AS PASSWORD,Role_user,IsAdmin,IsConnect " +
                    " from Utilisateur where id_user=@id_user;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@id_user", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        byte[] PhotoData = reader.IsDBNull(7) ? null : (byte[])reader[7]; // Check for NULL value and handle it accordingly
                                                                                          // status de utilisateur
                        string status_user = string.Empty;
                        switch (Convert.ToInt32(reader[13]))
                        {
                            case 1: status_user = "Connect"; break;
                            case 0: status_user = "Déconnect"; break;
                        }
                        user = new Utilisateur_Class(

                        (int)reader[0], (string)reader[1], (string)reader[2], (string)reader[3],
                             (string)reader[4], (string)reader[5], (string)reader[6], PhotoData,
                            (int)reader[8], (string)reader[9], (string)reader[10], reader[11].ToString(), (int)reader[12], status_user
                          );
                    }
                    reader.Close();
                }
                con.Close();
            }
            return user;
        }

        //select  photo user
        public static byte[] Select_photo_User(int id)
        {
            byte[] PhotoData = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();

                string sql = "select Photo_user  " +
                    " from Utilisateur where id_user=@id_user;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@id_user", id);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                       PhotoData = reader.IsDBNull(0) ? null : (byte[])reader[0]; // Check for NULL value and handle it accordingly
                    }
                    reader.Close();
                }
                con.Close();
            }
            return PhotoData;
        }

        //Autentification
        public static Utilisateur_Class Authenticate_user(string login, string password, int role)
        {

            Utilisateur_Class user = null;
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();

                string sql = "SELECT id_user, Role_user ,IsAdmin  ,Nom_user,Prenom_user,Login_user, CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('key_user',password_user)) AS PASSWORD " +
                          " FROM Utilisateur " +
                          " WHERE  CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('key_user',password_user)) COLLATE French_CS_AS =@password_user AND Login_user COLLATE French_CS_AS =@Login_user and Role_user=@Role_user and Status_Compte=1 ;";
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@Role_user", role);
                    commande.Parameters.AddWithValue("@Login_user", login);
                    commande.Parameters.AddWithValue("@password_user", password);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new Utilisateur_Class(
                             (int)reader[0], reader[1].ToString(), (int)reader[2], (string)reader[3], (string)reader[4]);
                    }
                    reader.Close();
                }
               
                con.Close();
                return user;
            }

        }

        //display users
        public static ObservableCollection<Utilisateur_Class> DisplayUser_Datagrid()
        {
            ObservableCollection<Utilisateur_Class> List_user = new ObservableCollection<Utilisateur_Class>();
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                // select the medcine et admini
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = " select id_user,Nom_user,Prenom_user , CIN_user,Phone_user, " +
                                            " Role_user,IsAdmin,IsConnect" +
                                            " from Utilisateur ;";
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        //selection le role
                        string role_user=string.Empty;
                        if (Convert.ToInt32(reader[6]) == 1)
                        {
                            role_user = "Administrateur";
                        }
                        else
                        {
                            switch (Convert.ToInt32(reader[5]))
                            {
                                case 1: role_user = "Medcine"; break;
                                case 2: role_user = "secretaire"; break;
                            }
                        }

                        // status de utilisateur
                        string isconnet = string.Empty;
                        switch (Convert.ToInt32(reader[7]))
                        {
                            case 1: isconnet = "Connect"; break;
                            case 0: isconnet = "Déconnect"; break;
                        }
                        List_user.Add(new Utilisateur_Class
                        {
                            id_user = (int)reader[0],
                            nom_user = (string)reader[1],
                            prenom_user = (string)reader[2],
                            CIN_user = (string)(reader[3]),
                            phone_user = (string)reader[4],
                            Role_user = role_user,
                            IsConnect = isconnet
                        }) ;
                    }
                    reader.Close();
                }
                con.Close();
            }
            return List_user;
        }

        //valide password
        public static int Valide_Login_Password(int id_u , string password , string login)
        {
            int nb = 0;
            string sql = string.Empty;
            if (id_u == -1)
            {
                //valider ou cas de nouveau utilisateur
                sql = " SELECT  count(id_user)  FROM Utilisateur " +
                        " WHERE  CONVERT(VARCHAR(Max), DECRYPTBYPASSPHRASE('key_user',password_user)) COLLATE French_CS_AS  = @password " +
                        "  AND  Login_user =  @Login_user; ";
            }
            else
            {
                sql = " SELECT  count(id_user)  FROM Utilisateur " +
                    " WHERE  CONVERT(VARCHAR(Max), DECRYPTBYPASSPHRASE('key_user',password_user)) COLLATE French_CS_AS   = @password " +
                    " AND  Login_user =  @Login_user AND id_user!= @id_user ;";
            }
            using (var con = ConnectDb.GetConnection())
            {
                con.Open();
                using (var commande = new SqlCommand())
                {
                    commande.Connection = con;
                    commande.CommandText = sql;
                    commande.Parameters.AddWithValue("@Login_user", login);
                    commande.Parameters.AddWithValue("@password", password);
                    commande.Parameters.AddWithValue("@id_user", id_u);
                    var reader = commande.ExecuteReader();
                    while (reader.Read())
                    {
                        nb = (int)reader[0];
                    }
                    reader.Close();
                }
                con.Close();
            }
            return nb;
        }     
    }
}
