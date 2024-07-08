using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using System.Threading.Tasks;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour detail_rendezVous.xaml
    /// </summary>
    public partial class detail_rendezVous : Window
    {
        ObservableCollection<RendezVous> p;
        public static int id_rendezVous { get; set; }
        public Utilisateur_Class user { get; set; }
        public DateTime date_save { get; set; }
        public int Patient_ID { get; set; }
        public bool IsLoad_fiche_patient { get; set; }
        public SqlTableDependency<DMRendezVous> RendezVousSqlTableDependency;
        public detail_rendezVous(int id_rdv, Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();

                id_rendezVous = id_rdv;
                this.user = user;
                //if role de user est secretaire
                if (Convert.ToInt32(user.Role_user) == 2) info.Visibility = Visibility.Collapsed;
                p = RendezVous.DisplayRendezVous(id_rendezVous);
                
                this.date_save = p[0].date;
                dateNaissance_patient.SelectedDate = p[0].date;
                if (!valiadion_function_date())
                {
                    Confirmer.IsEnabled = false;
                    Confirmer.Background = Brushes.Gray;
                }
                // if (p[0].date<DateTime.Now) dateNaissance_patient.IsEnabled= false;
                description.Text = p[0].description;

                patient.Text = p[0].nom + " " + p[0].prenom;
                picker.SelectedTime = new DateTime(1, 1, 1, p[0].heure.Hours, p[0].heure.Minutes, p[0].heure.Seconds);
                comboxType_Certificate.Text = p[0].type;
                /* ********************************************************************************************************* */
                block_date();
                IsLoad_fiche_patient = false;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
          
        }
        // -------------------------- SATRT EVENTS :  -------------------------------
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
               await Task.Run(() => Stop_TableRendezVous_Dependency());
            }
            catch (Exception)
            {
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               await Task.Run(() => Start_TableRendezVous_Dependency() );
            }
            catch (Exception)
            {
            }
        }

        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {
                DateTime time = (DateTime)picker.SelectedTime;
                real.Text = time.Hour.ToString() + " : " + time.Minute.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void dateNaissance_patient_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                bool x = valiadion_function_date();
            }
            catch (Exception)
            {

                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // verifier les horaires choisis!!

                if (picker.SelectedTime.Value.Hour > 20 || picker.SelectedTime.Value.Hour < 8 ||
                    picker.SelectedTime.Value.Hour < DateTime.Now.Hour &&
                        dateNaissance_patient.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    valide1.Visibility = Visibility.Visible;
                    valide1.Text = "horaire non valide!";
                    valide1.Foreground = Brushes.Red;

                }
                // si l'horaire est valide on continue le traitement...
                else
                {

                    if (dateNaissance_patient.SelectedDate != date_save)
                    {
                        if (!RendezVous.date_heure_valide(picker.SelectedTime.Value.ToLongTimeString(), dateNaissance_patient.SelectedDate.Value))
                        {
                            valide1.Visibility = Visibility.Visible;
                            valide1.Text = "horaire non valide!";
                            valide1.Foreground = Brushes.Red;
                        }
                        else
                        {
                            valide1.Visibility = Visibility.Visible;
                            valide1.Text = "horaire valide!";
                            valide1.Foreground = Brushes.Green;
                            // Ajout des donnée!
                            validation();
                        }
                    }
                    else
                    {
                        if (!RendezVous.date_heure_valide1(picker.SelectedTime.Value.ToLongTimeString(), dateNaissance_patient.SelectedDate.Value))
                        {
                            valide1.Visibility = Visibility.Visible;
                            valide1.Text = "horaire non valide!";
                            valide1.Foreground = Brushes.Red;
                            //   picker.SelectedTime = new DateTime(00, 00, 00, 00, 00, 00);

                        }
                        else
                        {
                            valide1.Visibility = Visibility.Visible;
                            valide1.Text = "horaire valide!";
                            valide1.Foreground = Brushes.Green;
                            // Ajout des donnée!
                            validation();
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //voir la fiche medical  de patient
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Patient_ID = RendezVous.get_patient(id_rendezVous);
                IsLoad_fiche_patient = true;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //supprimer rendez vous 
        private void supprimer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("voulez vous supprimer ce rendez vous?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    RendezVous.supprimerRendezVous(id_rendezVous);
                    new succes().ShowDialog();
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //quitter 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Voulez vous annuler!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- END EVENTS  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----


       
        //----------- start  sqlTable Dependecy : rendez Vous --------
        private void Start_TableRendezVous_Dependency()
        {
            try
            {
                if (RendezVousSqlTableDependency == null)
                {
                    RendezVousSqlTableDependency = new SqlTableDependency<DMRendezVous>(ConnectDb.GetConnectionstring(), "RendezVous");
                    RendezVousSqlTableDependency.OnChanged += Changed_RendezVous_Dependency;
                    RendezVousSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {
            }
        }
        private void Stop_TableRendezVous_Dependency()
        {
            try
            {
                if (RendezVousSqlTableDependency != null)
                {
                    RendezVousSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {
            }
        }
        private void Changed_RendezVous_Dependency(object sender, RecordChangedEventArgs<DMRendezVous> e)
        {
            try
            {
                DMRendezVous EntityrendezVous = e.Entity; 
                if ((e.ChangeType != ChangeType.None) && (EntityrendezVous.Id == id_rendezVous))
                {
                    switch (e.ChangeType)
                    {
                        case ChangeType.Update:
                            Dispatcher.Invoke(() =>
                            {
                                p = RendezVous.DisplayRendezVous(id_rendezVous);

                                this.date_save = p[0].date;
                                dateNaissance_patient.SelectedDate = p[0].date;
                                if (!valiadion_function_date())
                                {
                                    Confirmer.IsEnabled = false;
                                    Confirmer.Background = Brushes.Gray;
                                }
                                // if (p[0].date<DateTime.Now) dateNaissance_patient.IsEnabled= false;
                                description.Text = p[0].description;

                                patient.Text = p[0].nom + " " + p[0].prenom;
                                picker.SelectedTime = new DateTime(1, 1, 1, p[0].heure.Hours, p[0].heure.Minutes, p[0].heure.Seconds);
                                comboxType_Certificate.Text = p[0].type;
                                /* ********************************************************************************************************* */
                                block_date();
                            });
                            break;
                        case ChangeType.Delete:
                            Dispatcher.Invoke(() =>
                            {
                                 new Notification_window
                                                     (" Ce rendez vos   a ete  supprimer")
                                                     .ShowDialog();
                                 Close();
                            });
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //----------- end  sqlTable Dependecy : rendez Vous --------
        private void block_date()
        {
            DateTime systemDate = DateTime.Now;
            if (p[0].date.Date >= DateTime.Now.Date)
            {

                // Désactiver les dates inférieures à la date du système
                dateNaissance_patient.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, systemDate.AddDays(-1)));

                // Désactiver tous les dimanches jusqu'à une date future (vous pouvez ajuster cette date selon vos besoins)
                DateTime currentDate = DateTime.Now;
                while (currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    currentDate = currentDate.AddDays(1);
                }
                int i = 1;
                // Par exemple, désactiver les dimanches jusqu'à un an à partir de la date actuelle
                while (i++ < 52)
                {
                    dateNaissance_patient.BlackoutDates.Add(new CalendarDateRange(currentDate));
                    currentDate = currentDate.AddDays(7);
                }

            }
        }
      
        private void showErrorMessage(Border boxBorder, TextBlock Remarque, ValidationResult result)
        {
            if (result.IsValid == false) // si le contenu n'est pas valide
            {
                string s = "";  //recuperer lemessage d'erreurs
                foreach (var item in result.Errors) s += item.PropertyName + item.ErrorMessage;

                Remarque.Text = s;
                Remarque.Foreground = Brushes.Red;
                //   save_btn.IsEnabled = false;
                if (boxBorder != null)
                {
                    boxBorder.BorderBrush = Brushes.Red;
                }

            }
            else //si le contenu est valide
            {
                //save_btn.IsEnabled = true;

                Remarque.Text = "ce champs est valide";
                Remarque.Foreground = Brushes.Green;
                if (boxBorder != null)
                {
                    boxBorder.BorderBrush = Brushes.Green;
                }
            }
        }
        
        private bool valiadion_function_date()
        {
            Validation_RendezVous valideDate = new Validation_RendezVous(1);
            var resultDate = valideDate.Validate(Convert.ToDateTime(dateNaissance_patient.SelectedDate));

            Border? boxBorder = dateNaissance_patient.Template.FindName("BorderDate", dateNaissance_patient) as Border;
            showErrorMessage(boxBorder, RemarqueDateNaiss, resultDate);
            return resultDate.IsValid;
        }
        private void validation()
        {
            MessageBoxResult resultat = MessageBox.Show("Êtes-vous sûr de vouloir effectuer cette action ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {
                if (!valiadion_function_date())
                {
                    MessageBox.Show("Données Incompletes!!");

                }
                else
                {
                    try
                    {
                        RendezVous.ModifierReendezVous(id_rendezVous, dateNaissance_patient.SelectedDate, comboxType_Certificate.Text, description.Text, picker.SelectedTime.Value.ToLongTimeString());
                        // add notification
                        Notification_class notification_ =
                        new Notification_class(0, user.id_user, id_rendezVous, "rdv", DateTime.Now, " Modifié Un Rendez Vous  ");
                        notification_.Add_Notification();
                        new succes().ShowDialog();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Modification a généré une erreur inatendu");
                    }
                }
            }
        }
       
        //-----------------------  END  METHODES   ------------------------------------ ----
    }
}
