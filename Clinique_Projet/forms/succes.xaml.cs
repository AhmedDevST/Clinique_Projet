using System;
using System.Timers;
using System.Windows;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour succes.xaml
    /// </summary>
    public partial class succes : Window
    {
        
        public succes()
        {
            try
            {
                InitializeComponent();
                Loaded += VotreFenetre_Loaded;
            }
            catch (Exception)
            {
                Close();
            }
        }
        private void VotreFenetre_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            { 
                // Créez un objet Timer avec une durée spécifiée
                Timer timer = new Timer(2500); // Durée de 2.5 secondes (2500 millisecondes)
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
