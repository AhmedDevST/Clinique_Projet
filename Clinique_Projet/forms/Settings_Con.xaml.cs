using System;
using System.Configuration;
using System.Windows;


namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Settings_Con.xaml
    /// </summary>
    public partial class Settings_Con : Window
    {
       // Configuration config;
        public Settings_Con()
        {
            try
            {
                InitializeComponent();
                Load_Settings_Con();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
     
        private void Load_Settings_Con()
        {
            Nom_ser.Text = ConfigurationManager.AppSettings["Serveur"];
            user_ser.Text = ConfigurationManager.AppSettings["Utilisateur"];
            pass_ser.Text = ConfigurationManager.AppSettings["MotDePasse"];
            IP_ser.Text = ConfigurationManager.AppSettings["AdresseIP"];
            Nom_db.Text = ConfigurationManager.AppSettings["BaseDeDonnees"];

        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfigurationManager.AppSettings["Passeword_Admin"] == pass_admin.Text)
                {
                    MessageBoxResult res = MessageBox.Show("Voulez Vous Enregistrer ces informations?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["Serveur"].Value = Nom_ser.Text;
                        config.AppSettings.Settings["Utilisateur"].Value = user_ser.Text;
                        config.AppSettings.Settings["MotDePasse"].Value = pass_ser.Text;
                        config.AppSettings.Settings["AdresseIP"].Value = IP_ser.Text;
                        config.AppSettings.Settings["BaseDeDonnees"].Value = Nom_db.Text;

                        // Save changes to the configuration file
                        config.Save(ConfigurationSaveMode.Modified);

                        // Refresh the ConfigurationManager to reflect the changes
                        ConfigurationManager.RefreshSection("appSettings");

                        MessageBox.Show(" Les données sont  bien Enregistrer");
                        Close();
                    }     
                }
                else
                {
                    MessageBox.Show(" le mote de passe est incorrect !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
