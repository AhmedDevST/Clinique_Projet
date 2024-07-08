using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Clinique_Projet.forms
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception)
            {
                 MessageBox.Show("Opération d'entrée innatendu N M008 !!");

            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M009!!");
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Picture.Background = Convertir_Couleur("#FF75B6FF");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Medecin_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                PictureM.Background = Convertir_Couleur("#FF75B6FF");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Picture.Background = Brushes.Transparent;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Medecin_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                PictureM.Background = Brushes.Transparent;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                login.Background = Convertir_Couleur("#FF00ABD4");
                login.BorderBrush = Brushes.Blue;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void login_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                string hcolor = "#FF06CBFA";
                Color color = (Color)ColorConverter.ConvertFromString(hcolor);
                SolidColorBrush brush = new SolidColorBrush(color);
                login.Background = brush;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                login_function();
            }
            catch (Exception )
            {
                MessageBox.Show("Opération d'entrée innatendu !!:");
            }
        }
        private void Erreur_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Erreur.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter) login_function();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M011 !!");
            }
        }
        private void settings_con_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new Settings_Con().ShowDialog();
        }

        // -------------------------- END EVENTS :  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private SolidColorBrush Convertir_Couleur(string hcolor)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hcolor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
        }
        private void login_function()
        {
            if ((Matricule.Text.Length != 0) && (Password.Password.ToString().Length != 0) && ((check_sect.IsChecked == true) || (check_med.IsChecked == true)))
            {
                int Role = -1;
                if (check_sect.IsChecked == true)
                {
                    
                    Role = 2;
                }
                else if (check_med.IsChecked == true)
                {
                    Role = 1;
                }
                Utilisateur_Class user = Utilisateur_Class.Authenticate_user(Matricule.Text, Password.Password.ToString(), Role);
                if (user != null)
                {
                    switch (Convert.ToInt32(user.Role_user))
                    {
                        case 1: new Acceuil_Docteur(user).Show();
                            new welcome(user).ShowDialog();
                            break;
                        case 2: new Acceuil(user).Show();
                            new welcome(user).ShowDialog();
                            break;
                        default:this.Close(); break;
                    }   
                    this.Close();
                }
                else
                {
                    
                    erreur_msg(" ");
                }
            }
            else erreur_msg("Un ou plusieurs champs sont  vides!!");
        }
        //erreur message du login
        private void erreur_msg(string message)
        {
            if(message != " ")  Erreur_msg.Text = message;
            Erreur.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 15;
            animation.To = Erreur.ActualWidth;
            animation.Duration = TimeSpan.FromSeconds(0.5);
            Erreur.Visibility = Visibility.Visible;
            Erreur.BeginAnimation(Border.WidthProperty, animation);
        }
        
        //-----------------------  END  METHODES   ------------------------------------ ----
    }
}
