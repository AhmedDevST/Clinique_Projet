using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Parametres_Medicals.xaml
    /// </summary>
    public partial class Parametres_Medicals : UserControl
    {
        public Utilisateur_Class user;
        public Parametres_Medicals(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                Droits_user();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        // Parameters Analyses
        private void ParameterAnalyse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Controle_Parametres.Children.Clear();
                Parametre_Analyses a = new Parametre_Analyses(user);
                Controle_Parametres.Children.Add(a);
                clicked(ParameterAnalyse_btn);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //Parameters medicaments
        private void medicament_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Controle_Parametres.Children.Clear();
                Parametre_Medicament a = new Parametre_Medicament(user);
                Controle_Parametres.Children.Add(a);
                clicked(medicament_btn);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //plus de parametres
        private void ParameterAutre_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Controle_Parametres.Children.Clear();
                More_Parametre a = new More_Parametre(user);
                Controle_Parametres.Children.Add(a);
                clicked(ParameterAutre_btn);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //parametres utilisateur
        private void AccountSettigns_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Controle_Parametres.Children.Clear();
                Acount_Settigns_Control a = new Acount_Settigns_Control(user);
                Controle_Parametres.Children.Add(a);
                clicked(AccountSettigns_btn);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
        
        // -------------------------- END EVENTS  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void Droits_user()
        {
            if (user.IsAdmin == 0)
            {
                AccountSettigns_btn.Visibility = Visibility.Collapsed;
            }
            Controle_Parametres.Children.Clear();
            Parametre_Analyses a = new Parametre_Analyses(user);
            Controle_Parametres.Children.Add(a);
            clicked(ParameterAnalyse_btn);
        }

        private SolidColorBrush Convertir_Couleur(string hcolor)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hcolor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
        }
        private void clicked(Button b)
        {
            try
            {
                // border brush
                medicament_btn.BorderBrush = Brushes.Transparent;
                ParameterAutre_btn.BorderBrush = Brushes.Transparent;
                ParameterAnalyse_btn.BorderBrush = Brushes.Transparent;
                AccountSettigns_btn.BorderBrush = Brushes.Transparent;
                
                // background
                medicament_btn.Background = Brushes.Transparent;
                ParameterAnalyse_btn.Background = Brushes.Transparent;
                ParameterAutre_btn.Background = Brushes.Transparent;
                AccountSettigns_btn.Background = Brushes.Transparent;

                medicament_btn.Foreground = Brushes.Gray;
                ParameterAnalyse_btn.Foreground = Brushes.Gray;
                ParameterAutre_btn.Foreground = Brushes.Gray;
                AccountSettigns_btn.Foreground = Brushes.Gray;
            
                b.Background = Convertir_Couleur("#007bff");
                b.BorderBrush = Convertir_Couleur("#007bff");
                b.Foreground = Brushes.White;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }      
        }

        //-----------------------  END  METHODES    ------------------------------------ ----
    }
}
