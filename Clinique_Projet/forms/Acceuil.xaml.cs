using Clinique_Projet.connectionDb;
using Clinique_Projet.Controlers;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Acceuil.xaml
    /// </summary>
    public partial class Acceuil : Window
    {
        public static bool rendezVous=false,patien=false,consult= false,cert =false;

        public Utilisateur_Class user;

        // DMUtilisateur : recureper les modifcations sur la table Utilisateur
        public SqlTableDependency<DMUtilisateur> UserSqlTableDependency;

        public Acceuil(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                menu(test, test_pin);
                home home = new home(user);
                controler.Children.Clear();
                controler.Children.Add(home);
                rendezVous = true;

                //update status :connect
                Utilisateur_Class.Update_Status_User(user.id_user, 1);
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M089 !!");
            }
        }
        // ----------- start sql dependecy : Utilisateur : le cas changer status deactiver le compte ou supprimer ou changer le role  -------------
        private void Start_TableUser_Dependency()
        {
            try
            {
                if (UserSqlTableDependency == null)
                {
                    UserSqlTableDependency = new SqlTableDependency<DMUtilisateur>(ConnectDb.GetConnectionstring(), "Utilisateur");
                    UserSqlTableDependency.OnChanged += Changed_User_Dependency;
                    UserSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_User_Dependency(object sender, RecordChangedEventArgs<DMUtilisateur> e)
        {
            try
            {
                DMUtilisateur EntityUser = e.Entity;
                if (EntityUser.id_user == user.id_user)
                {
                    switch (e.ChangeType)
                    {
                        case ChangeType.Update:
                            Dispatcher.Invoke(() =>
                            {
                                // le role est changer  ou  le status de compte  est deactiver 
                                if ((EntityUser.Role_user != Convert.ToInt32(user.Role_user))
                                  || (EntityUser.Status_Compte == 0))
                                {
                                    new Notification_window(" la session est fermer  ")
                                                      .ShowDialog();
                                    Close();
                                }
                            });
                            break;

                        case ChangeType.Delete:
                            Dispatcher.Invoke(() =>
                            {
                                new Notification_window(" la session est fermer  ")
                                                      .ShowDialog();
                                Close();

                            });
                            break;
                    }
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_TableConsultation_Dependency()
        {
            try
            {
                if (UserSqlTableDependency != null)
                {
                    UserSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy : Utilisateur -------------


        private SolidColorBrush Convertir_Couleur(string hcolor)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hcolor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
        }
   
        private void Border(int a, Border border)
        {
            try 
            { 
                if (a == 0)
                {
                    border.Background = Convertir_Couleur("#FFF5F5F5");
                    border.BorderBrush = Brushes.Gold;
                }
                else
                {
                    border.Background = Brushes.AliceBlue;
                    border.BorderBrush = Brushes.Gray;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
    
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rendezVous = true;  
        }
        public void ChangerControleur(UserControl nouveauControleur)
        {
            // Effacez le contenu existant de la fenêtre
            controler.Children.Clear();

            // Ajoutez le nouveau contrôleur à la fenêtre
           controler.Children.Add(nouveauControleur);
        }

     
        //certificates
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Modéle_certificat p = new Modéle_certificat(user);
                controler.Children.Add(p);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }      
        }

        // rendez vous
        private void test_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(test,test_pin);
                home home = new home(user);
                controler.Children.Clear();
                controler.Children.Add(home);
                rendezVous = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }       
        }

        private void test_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!rendezVous)
                    test.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
           
        }

        private void test_MouseLeave(object sender, MouseEventArgs e)
        {
            try { 
                if(!rendezVous)
                test.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void menu(StackPanel stack, Image img)
        {
            try
            {
                test.Background = Convertir_Couleur("#FF7564CD");
                patient.Background = Convertir_Couleur("#FF7564CD");
                certificat.Background = Convertir_Couleur("#FF7564CD");
                consultation.Background = Convertir_Couleur("#FF7564CD");
                rendezVous = false;
                patien = false;
                cert = false;
                consult = false;
                test_pin.Visibility = Visibility.Collapsed;
                patient_pin.Visibility = Visibility.Collapsed;
                consultation_pin.Visibility = Visibility.Collapsed;
                certificat_pin.Visibility = Visibility.Collapsed;
                stack.Background = Convertir_Couleur("#FFB4A6FE");
                img.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }


        }

        private void patient_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!patien)
                {
                    patient.Background = Convertir_Couleur("#FFB4A6FE");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }            
        }

        private void patient_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!patien)
                    patient.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }          
        }
        private void consultation_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!consult)
                    consultation.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
           
        }

        private void consultation_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!consult)
                    consultation.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }        
        }
        private void certificat_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!cert)
                    certificat.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }

        //deconnexion
        private void disconnect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Confirmation(user).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //gestion patient
        private void patient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(patient,patient_pin);
                controler.Children.Clear();
                Patient_Controlle p = new Patient_Controlle(user);
                controler.Children.Add(p);
                patien = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }

        // certifcate
        private void certificat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(certificat,certificat_pin);
                cert = true;
                Modéle_certificat c = new Modéle_certificat(user);
                controler.Children.Clear();
                controler.Children.Add(c);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
            
        }

        private void certificat_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!cert)
                    certificat.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }

        //gestion consultation
        private void consultation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(consultation,consultation_pin);
                consult = true;
                controler.Children.Clear();
                controler.Children.Add(new consultation_sec(user));
                
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }    
        }

        //close window: close thraed and listencer
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await Task.Run(() => Stop_TableConsultation_Dependency() );
                //update status :dconnect
                Utilisateur_Class.Update_Status_User(user.id_user, 0);
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                test.Height = 50;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               await Task.Run(() => Start_TableUser_Dependency());
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite !!");
            }
        }

        private void disconnect_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                disconnect.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
         
        }

        private void disconnect_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                    disconnect.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
           
        }
        private void ActualisationRequiseEventHandler(object sender, EventArgs e)
        {
            // Logique d'actualisation du contrôle utilisateur ici
        }
    }
}
