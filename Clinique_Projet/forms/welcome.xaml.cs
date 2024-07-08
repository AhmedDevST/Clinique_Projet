using Clinique_Projet.Modal;
using System;
using System.Timers;
using System.Windows;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour welcome.xaml
    /// </summary>
    public partial class welcome : Window
    {
        public welcome(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Loaded += VotreFenetre_Loaded;
                if (Convert.ToInt32(user.Role_user) == 1)
                    text.Text = "Bonjour Dr." + user.nom_user.ToUpper() + " " + user.prenom_user.ToUpper();
                else text.Text = "Bonjour une autre fois !"; 
            }
            catch(Exception)
            {
                Close();
            }       
        }
        private void VotreFenetre_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Créez un objet Timer avec une durée spécifiée
                Timer timer = new Timer(2000); // Durée de 2.5 secondes (2500 millisecondes)
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }
            catch (Exception)
            {
                Close();
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Fermez la fenêtre sur le thread de l'interface utilisateur
                Dispatcher.Invoke(() =>
                {
                    Close();
                });
            }
            catch (Exception)
            {
                Close();
            }
        }
    }
}
