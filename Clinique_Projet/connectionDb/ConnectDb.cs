using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

namespace Clinique_Projet.connectionDb
{
    public class ConnectDb
    {
        ConnectDb() { }

        public static String GetConnectionstring()
        {
            string serveur = ConfigurationManager.AppSettings["Serveur"];
            string baseDeDonnees = ConfigurationManager.AppSettings["BaseDeDonnees"];
            string utilisateur = ConfigurationManager.AppSettings["Utilisateur"];
            string motDePasse = ConfigurationManager.AppSettings["MotDePasse"];
            string adresseIP = ConfigurationManager.AppSettings["AdresseIP"];

            // Utilisez ces valeurs pour construire votre chaîne de connexion.
            string maConnexion = @"Data Source = " + adresseIP + " ; server = " + serveur + "; Initial Catalog = " + baseDeDonnees + " ; User Id = " + utilisateur + " ; Password = " + motDePasse + " ;" ;
            return maConnexion;   
        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectDb.GetConnectionstring());

            //return @"server=DESKTOP-OPP1BT4\SQLEXPRESS;database=MedCab;integrated security=true";

            //@"server=PACKARDBELL\SQLEXPRESS;database=db_cout;integrated security=true"
            //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\C#\WindowsFormsApp3\WindowsFormsApp3\db_cout.mdf;Integrated Security=True"
            //@"server=DESKTOP-IKVK4MR;database=db_cout;integrated security=true"
            //@"server=ADMIN;database=db_cout;integrated security=true"
            //@"server=DESKTOP-OPP1BT4;database=MedalDataBase;integrated security=true"
            //@"server=MOHAMED-EL-GHAB;database=MedalDataBase;integrated security=true"
            //(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pcNB2\Documents\Nouveau dossier\Clinique_Projet\Clinique_Projet\Clinique_Projet\Cab_Med.mdf; integrated security = true")
            //    return new SqlConnection(@"server=DESKTOP-OPP1BT4\SQLEXPRESS;database=MedCab;integrated security=true");
            //return new SqlConnection(@"Server=DESKTOP-RR36GRB\SQLEXPRESS;Initial Catalog=Cab_Med;User ID=sa;Password=654123");
            // return new SqlConnection(@"Data Source=192.168.2.1;server=DESKTOP-OPP1BT4\SQLEXPRESS;Initial Catalog=Cab_Med;User ID=sa;Password=123456");

            //return new SqlConnection(@"server=DESKTOP-RR36GRB\SQLEXPRESS;database=Cab_Med;integrated security=true");

            /* string serveur = ConfigurationManager.AppSettings["Serveur"];
             string baseDeDonnees = ConfigurationManager.AppSettings["BaseDeDonnees"];
             string utilisateur = ConfigurationManager.AppSettings["Utilisateur"];
             string motDePasse = ConfigurationManager.AppSettings["MotDePasse"];
             string adresseIP = ConfigurationManager.AppSettings["AdresseIP"];

             // Utilisez ces valeurs pour construire votre chaîne de connexion.
             string maConnexion = $"Data Source = {adresseIP} ; server = {serveur}; Initial Catalog = {baseDeDonnees} ; User Id = {utilisateur} ; Password = {motDePasse};";
             // string maConnexion = ConfigurationManager.ConnectionStrings["MaConnexion"].ConnectionString;
             return new SqlConnection(maConnexion);*/

            // Utilisez maConnexion dans votre code pour établir la connexion à la base de données.



        }
    }
}
