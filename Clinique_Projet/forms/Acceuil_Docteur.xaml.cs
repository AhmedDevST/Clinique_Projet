using Clinique_Projet.connectionDb;
using Clinique_Projet.Controlers;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace Clinique_Projet.forms
{
    public partial class Acceuil_Docteur : Window
    {
        public static bool rendezVous = true, patien = false, consult = false, cert = false, dash = false;
        public Utilisateur_Class user;
        public int NUMBER_NOTIFICATION = 0;

        // DMUtilisateur : recureper les modifcations sur la table Utilisateur
        public SqlTableDependency<DMUtilisateur> UserSqlTableDependency;

        // DMNotification_Consultation : recureper les modifcations sur la table Notification_Consultation
        public SqlTableDependency<DMNotification_Consultation> Notification_ConsultationSqlTableDependency;

        // DMNotification_patient : recureper les modifcations sur la table Notification_patient
        public SqlTableDependency<DMNotification_patient> Notification_patientSqlTableDependency;

        // DMNotification_RendezVous : recureper les modifcations sur la table Notification_RendezVous
        public SqlTableDependency<DMNotification_RendezVous> Notification_RendezVousSqlTableDependency;

        public Acceuil_Docteur(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                Controlers.Children.Clear();
                Controlers.Children.Add(new home(user));
                //determminer droit de user
                Droits_User();
                //update status :connect
                Utilisateur_Class.Update_Status_User(user.id_user, 1);
                load_info_user();
                menu(test, test_pin);
                rendezVous = true;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M005 !!");
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
            catch (Exception )
            {
                
            }
        }
        private void Changed_User_Dependency(object sender, RecordChangedEventArgs<DMUtilisateur> e)
        {
            try
            {
                DMUtilisateur EntityUser= e.Entity;
                if (EntityUser .id_user == user.id_user)
                {
                    switch (e.ChangeType)
                    {
                        case ChangeType.Update:
                            Dispatcher.Invoke(() =>
                            {
                                // le role est changer  ou  le status de compte  est deactiver 
                                if ( (EntityUser.Role_user != Convert.ToInt32(user.Role_user)) 
                                  || (EntityUser.Status_Compte == 0) )
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
        private void Stop_TableUser_Dependency()
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

        // ----------- start sql dependecy : Notification_Consultation  -------------
        private void Start_TableNotification_Consultation_Dependency()
        {
            try
            {
                if (Notification_ConsultationSqlTableDependency == null)
                {
                    Notification_ConsultationSqlTableDependency = new SqlTableDependency<DMNotification_Consultation>(ConnectDb.GetConnectionstring(), "Notification_Consultation");
                    Notification_ConsultationSqlTableDependency.OnChanged += Changed_Notification_Consultation_Dependency;
                    Notification_ConsultationSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Notification_Consultation_Dependency(object sender, RecordChangedEventArgs<DMNotification_Consultation> e)
        {
            try
            {
                if (e.ChangeType == ChangeType.Insert)
                {
                    Dispatcher.Invoke(() =>
                    {
                        NUMBER_NOTIFICATION++;
                        Number_notification.Text = NUMBER_NOTIFICATION.ToString();
                        Border_notification.Visibility = Visibility.Visible;
                    });
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_TableNotification_Consultation_Dependency()
        {
            try
            {
                if (Notification_ConsultationSqlTableDependency != null)
                {
                    Notification_ConsultationSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }
        // ----------- End sql dependecy : Notification_Consultation -------------


        // ----------- start sql dependecy : Notification_patient  -------------
        private void Start_TableNotification_patient_Dependency()
        {
            try
            {
                if (Notification_patientSqlTableDependency == null)
                {
                    Notification_patientSqlTableDependency = new SqlTableDependency<DMNotification_patient>(ConnectDb.GetConnectionstring(), "Notification_patient");
                    Notification_patientSqlTableDependency.OnChanged += Changed_Notification_patient_Dependency;
                    Notification_patientSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Notification_patient_Dependency(object sender, RecordChangedEventArgs<DMNotification_patient> e)
        {
            try
            {
                if (e.ChangeType == ChangeType.Insert)
                {
                    Dispatcher.Invoke(() =>
                    {
                        NUMBER_NOTIFICATION++;
                        Number_notification.Text = NUMBER_NOTIFICATION.ToString();
                        Border_notification.Visibility = Visibility.Visible;
                    });
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_TableNotification_patient_Dependency()
        {
            try
            {
                if (Notification_patientSqlTableDependency != null)
                {
                    Notification_patientSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }
        // ----------- End sql dependecy : Notification_patient -------------

        // ----------- start sql dependecy : Notification_RendezVous  -------------
        private void Start_TableNotification_RendezVous_Dependency()
        {
            try
            {
                if (Notification_RendezVousSqlTableDependency == null)
                {
                    Notification_RendezVousSqlTableDependency = new SqlTableDependency<DMNotification_RendezVous>(ConnectDb.GetConnectionstring(), "Notification_RendezVous");
                    Notification_RendezVousSqlTableDependency.OnChanged += Changed_Notification_RendezVous_Dependency;
                    Notification_RendezVousSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Notification_RendezVous_Dependency(object sender, RecordChangedEventArgs<DMNotification_RendezVous> e)
        {
            try
            {
                if (e.ChangeType == ChangeType.Insert)
                {
                    Dispatcher.Invoke(() =>
                    {
                        NUMBER_NOTIFICATION++;
                        Number_notification.Text = NUMBER_NOTIFICATION.ToString();
                        Border_notification.Visibility = Visibility.Visible;
                    });
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_Notification_RendezVous_Dependency()
        {
            try
            {
                if (Notification_RendezVousSqlTableDependency != null)
                {
                    Notification_RendezVousSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }
        // ----------- End sql dependecy : Notification_Consultation -------------


        private void load_info_user()
        {
            try 
            {
                //nom utilisdateur
                Name_docteur.Text = "DR . " + user.nom_user + " " + user.prenom_user;

                //photo de user
                byte[] PhotoData = Utilisateur_Class.Select_photo_User(user.id_user);
                if (PhotoData!= null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = new MemoryStream(PhotoData);
                    bitmapImage.EndInit();
                    photo_user.ImageSource = bitmapImage;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M006 !!");
            }
        }
        private void Droits_User()
        {
            // seul admin a le droit de voir tabelau de bord et  settings user
            if (user.IsAdmin == 0) 
            { 
                fifth.Visibility = Visibility.Collapsed;
                Isadmin_text.Text = "Docteur";
            }
            else Isadmin_text.Text = "Admin";
        }

        private void menu(StackPanel stack, Image img)
        {
            try
            {
                test.Background = Convertir_Couleur("#FF7564CD");
                patient.Background = Convertir_Couleur("#FF7564CD");
                certificat.Background = Convertir_Couleur("#FF7564CD");
                consultation.Background = Convertir_Couleur("#FF7564CD");
                dashboard.Background = Convertir_Couleur("#FF7564CD");
                rendezVous = false;
                patien = false;
                cert = false;
                consult = false;
                test_pin.Visibility = Visibility.Collapsed;
                patient_pin.Visibility = Visibility.Collapsed;
                consultation_pin.Visibility = Visibility.Collapsed;
                certificat_pin.Visibility = Visibility.Collapsed;
                dashboard_pin.Visibility = Visibility.Collapsed;
                stack.Background = Convertir_Couleur("#FFB4A6FE");
                img.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N: M103");
            }

        }

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = sidebare.ActualWidth;
                animation.To = 40;
                animation.Duration = TimeSpan.FromSeconds(0.3);
                sidebare.BeginAnimation(Border.WidthProperty, animation);
                sidebare.HorizontalAlignment = HorizontalAlignment.Left;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M509 !!");
            }       
        }
      
        //gestion patient
        private void patient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(patient,patient_pin);
                Controlers.Children.Clear();
                Controlers.Children.Add(new PatientControlle_Medecin(user));
                patien = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M980 !!");
            }
        }

        // gestion rendez vous
        private void test_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(test, test_pin);
                Controlers.Children.Clear();
                Controlers.Children.Add(new home(user));
                rendezVous = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M981 !!");
            }    
        }

        // gestion consultation 
        private void consultation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(consultation,consultation_pin);
                Controlers.Children.Clear();
                Controlers.Children.Add(new GestionConsultation_Control(user));
                consult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M987 !!");
            }     
        }

        //tableu de bords
        private void dashboard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menu(dashboard,dashboard_pin);
                Controlers.Children.Clear();
                Controlers.Children.Add(new Dashbord_Interface());
                dash = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M988 !!");
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
                MessageBox.Show("Opération d'entrée innatendu N M989 !!");
            }
        }
        
        //fermer
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
             try
             {
                await Task.WhenAll(
                    Task.Run(() => Stop_TableUser_Dependency()),
                    Task.Run(() => Stop_TableNotification_Consultation_Dependency()),
                    Task.Run(() => Stop_TableNotification_patient_Dependency()),
                    Task.Run(() => Stop_Notification_RendezVous_Dependency())
                );
                //update status :dconnect
                Utilisateur_Class.Update_Status_User(user.id_user, 0);
             }
            catch (Exception)
             {
                 MessageBox.Show("Opération d'entrée innatendu N M105 !!");
             }
        }

        // CErtificate
        private void certificat_MouseDown(object sender, MouseButtonEventArgs e)
        {
           try
            {
                menu(certificat,certificat_pin);
                Controlers.Children.Clear();
                Controlers.Children.Add(new Modéle_certificat(user));
                cert = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M106 !!");
               
            }
        }

        public void ChangerControleur(UserControl nouveauControleur)
        {
            try
            {
                // Effacez le contenu existant de la fenêtre
                Controlers.Children.Clear();
                // Ajoutez le nouveau contrôleur à la fenêtre
            Controlers.Children.Add(nouveauControleur);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M109 !!");
            }      
        }

        private SolidColorBrush Convertir_Couleur(string hcolor)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hcolor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
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
                MessageBox.Show("Une opération innatendu s'est produite N M107 ");
            }
        }

        private void test_MouseLeave(object sender, MouseEventArgs e)
        {
            try 
            { 
                if (!rendezVous)
                    test.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération d'entrée/sortie innatendue s'est produite");
            }
        }

        private void patient_MouseEnter(object sender, MouseEventArgs e)
        {
            try 
            { 
                 if (!patien) patient.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite !!");
            }
        }

        private void patient_MouseLeave(object sender, MouseEventArgs e)
        {
            try 
            { 
                 if (!patien) patient.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N M900 !!");
            }
        }

        private void consultation_MouseEnter(object sender, MouseEventArgs e)
        {
            try 
            { 
                   if (!consult) consultation.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N M910");
            }
        }

        private void consultation_MouseLeave(object sender, MouseEventArgs e)
        {
            try 
            { 
                if (!consult) consultation.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N M716 !!");
            }
        }

        private void certificat_MouseEnter(object sender, MouseEventArgs e)
        {
            try 
            { 
                if (!cert) certificat.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N M771 !!");
            }
        }

        private void certificat_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            { 
                if (!cert) certificat.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite N M987 !!");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                     Task.Run(() => Start_TableUser_Dependency()),
                     Task.Run(() => Start_TableNotification_Consultation_Dependency()),
                     Task.Run(() => Start_TableNotification_patient_Dependency()),
                     Task.Run(() => Start_TableNotification_RendezVous_Dependency())
                 );
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite !!");
            }
        }

        private void dashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            try 
            { 
                 if (!dash) dashboard.Background = Convertir_Couleur("#FFB4A6FE");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite !!");
            }
        }

        private void dashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            try 
            { 
                 if (!dash) dashboard.Background = Convertir_Couleur("#FF7564CD");
            }
            catch (Exception)
            {
                MessageBox.Show("Une opération innatendu s'est produite");
            }
        }

        //parametres
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                if (acceuil != null)
                {
                    acceuil.ChangerControleur(new Parametres_Medicals(user));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
       
        //notifications
        private void notify_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                if (acceuil != null)
                {
                    acceuil.ChangerControleur(new Notification_Control(user));
                    Border_notification.Visibility = Visibility.Collapsed;
                    NUMBER_NOTIFICATION = 0;
                    Number_notification.Text = NUMBER_NOTIFICATION.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu  N M126 !!");
            }
        }

    }
}
