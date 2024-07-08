using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Prochaine_RDV.xaml
    /// </summary>
    public partial class Prochaine_RDV : Window
    {
        public int Patient_ID { get; set; }
        public int Consultation_ID { get; set; }
        public RendezVous Obj_RendezVous;
        public Prochaine_RDV(int patient_ID,int id_consult)
        {
            try
            {
                InitializeComponent();
                Patient_ID = patient_ID;
                Consultation_ID = id_consult;
                Initialiser_RDV();
            }catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        // ajouter rdv
        private void AddRDV_btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!valiadion_function_date())
                {
                    MessageBox.Show("Données Incompletes!!");
                }
                else
                {
                    string heure_rendezVous;
                    // si le temps n'est pas selectionner on selectionne le temps du systeme;
                    switch (picker.SelectedTime)
                    {
                        case null:
                            if (DateTime.Now.Hour < 8 || DateTime.Now.Hour > 20)
                            {
                                valide1.Visibility = Visibility.Visible;
                                valide1.Text = "horaire non valide!";
                                valide1.Foreground = Brushes.Red;
                            }
                            else
                            {
                                picker.SelectedTime = DateTime.Now;
                                heure_rendezVous = DateTime.Now.ToLongTimeString();
                                validation(heure_rendezVous);
                            }

                            break;
                        default:
                            heure_rendezVous = picker.SelectedTime.Value.ToLongTimeString();
                            validation(heure_rendezVous);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N1 !!");
            }
        }

        //modifier rdv
        private void EditRDV_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // verifier les horaires choisis!!

                if (picker.SelectedTime.Value.Hour > 20 || picker.SelectedTime.Value.Hour < 8 ||
                    picker.SelectedTime.Value.Hour < DateTime.Now.Hour &&
                        date_rdv.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    valide1.Visibility = Visibility.Visible;
                    valide1.Text = "horaire non valide!";
                    valide1.Foreground = Brushes.Red;

                }
                // si l'horaire est valide on continue le traitement...
                else
                {

                    if (date_rdv.SelectedDate != Obj_RendezVous.date)
                    {
                        if (!RendezVous.date_heure_valide(picker.SelectedTime.Value.ToLongTimeString(), date_rdv.SelectedDate.Value))
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
                            validation_update();
                        }
                    }
                    else
                    {
                        if (!RendezVous.date_heure_valide1(picker.SelectedTime.Value.ToLongTimeString(), date_rdv.SelectedDate.Value))
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
                            validation_update();
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //supprimer rdv
        private void DeleteRDV_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("voulez vous supprimer ce rendez vous?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    RendezVous.supprimerRendezVous(Obj_RendezVous.id);
                    new succes().ShowDialog();
                    initiliser_operation();
                    datagrid_Prdv.ItemsSource = Prochaine_RDV_class.Display_PRDV(Consultation_ID);
                    show_image_aucun_ProchaineRDV();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler rdv
        private void AnnulerRDV_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initiliser_operation();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // show info rdv
        private void show_rdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Prdv.SelectedItem != null)
                {
                    Obj_RendezVous = (RendezVous)datagrid_Prdv.SelectedItem;
                    description.Text = Obj_RendezVous.description;
                    date_rdv.SelectedDate = Obj_RendezVous.date;
                    picker.SelectedTime = new DateTime(1, 1, 1, Obj_RendezVous.heure.Hours, Obj_RendezVous.heure.Minutes, Obj_RendezVous.heure.Seconds);

                    //enable edit and delete
                    //si la date est superieur a date now => enable modifer
                    if (valiadion_function_date()) EditRDV_btn.IsEnabled = true;
                    DeleteRDV_btn.IsEnabled = true;
                    AddRDV_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu : La date du rendez vous est inconvennable !!");
            }
        }


        // valide la date de rendez vous
        private void date_rdv_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                valiadion_function_date();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // date et heure de rdv
        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {
                DateTime time = (DateTime)picker.SelectedTime;
                real.Text = time.Hour.ToString() + " : " + time.Minute.ToString();

                // faire un test sur la validitée de l'heure choisie ==> si cette heure est déja reservé ou s'elle respecte le temps du travail.
                if (time.Hour < 8 || time.Hour > 20 ||
                        !RendezVous.date_heure_valide(picker.SelectedTime.Value.ToLongTimeString(), date_rdv.SelectedDate.Value))
                {
                    valide1.Visibility = Visibility.Visible;
                    valide1.Text = "horaire non valide!";
                    valide1.Foreground = Brushes.Red;
                    //picker.SelectedTime = new DateTime(00, 00, 00,00,00,00);

                }
                else
                {
                    valide1.Visibility = Visibility.Visible;
                    valide1.Text = "horaire valide!";
                    valide1.Foreground = Brushes.Green;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }



        // -------------------------- END EVENTS   -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //initiliser les champs de rdv
        private void Initialiser_RDV()
        {
            try
            {
                //load data prochaine rendez vous
                datagrid_Prdv.ItemsSource = Prochaine_RDV_class.Display_PRDV(Consultation_ID);
                DeleteRDV_btn.IsEnabled = false;
                EditRDV_btn.IsEnabled = false;
                show_image_aucun_ProchaineRDV();

                if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                    date_rdv.SelectedDate = DateTime.Now;
                else
                    date_rdv.SelectedDate = DateTime.Now.AddDays(1);

                //annuler.Click += Button_Click;
                // Récupérer la date du système
                DateTime systemDate = DateTime.Now;

                // Désactiver les dates inférieures à la date du système
                date_rdv.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, systemDate.AddDays(-1)));

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
                    date_rdv.BlackoutDates.Add(new CalendarDateRange(currentDate));
                    currentDate = currentDate.AddDays(7);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //valide add rdv
        private void validation(string heure_rendezVous)
        {
            if (!RendezVous.date_heure_valide(heure_rendezVous, date_rdv.SelectedDate.Value))
            {
                valide1.Visibility = Visibility.Visible;
                valide1.Text = "horaire non valide!";
                valide1.Foreground = Brushes.Red;
                //   picker.SelectedTime = new DateTime(00, 00, 00, 00, 00, 00);

            }
            else
            {
                MessageBoxResult resultat = MessageBox.Show("Êtes-vous sûr de vouloir effectuer cette action ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultat == MessageBoxResult.Yes)
                {
                    if (picker.SelectedTime.Value.Hour < DateTime.Now.Hour &&
                        date_rdv.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {

                        valide1.Visibility = Visibility.Visible;
                        valide1.Text = "Veuillez choisir un horaire valide!!";
                        valide1.Foreground = Brushes.Red;
                    }
                    else
                    {

                        valide1.Visibility = Visibility.Visible;
                        valide1.Text = "horaire valide!";
                        valide1.Foreground = Brushes.Green;
                        //ajouter dans la table rendez vous
                        RendezVous.ajouter_RendezVous2(Convert.ToDateTime(date_rdv.SelectedDate), heure_rendezVous,
                            "Controle", Patient_ID, description.Text);

                        // selection la nouveau rendez vous 
                        int id_new_rdv = Prochaine_RDV_class.Selection_PRDV(
                            Convert.ToDateTime(date_rdv.SelectedDate), heure_rendezVous,
                            "Controle", Patient_ID, description.Text
                            );

                        //insert dans la table prchaine rendez vous
                        new Prochaine_RDV_class(id_new_rdv, Consultation_ID).ADD_Prochaine_rdv();

                        //load list prochaine rendez vous 
                        datagrid_Prdv.ItemsSource = Prochaine_RDV_class.Display_PRDV(Consultation_ID);
                        new succes().ShowDialog();
                        initiliser_operation();
                        show_image_aucun_ProchaineRDV();
                    }
                }
            }
        }

        //valide update rdv
        private void validation_update()
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
                        RendezVous.ModifierReendezVous(Obj_RendezVous.id, date_rdv.SelectedDate, "controle", description.Text, picker.SelectedTime.Value.ToLongTimeString());
                        new succes().ShowDialog();
                        initiliser_operation();
                        datagrid_Prdv.ItemsSource = Prochaine_RDV_class.Display_PRDV(Consultation_ID);
                        show_image_aucun_ProchaineRDV();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Modification a généré une erreur inatendu");
                    }
                }
            }
        }

        private void initiliser_operation()
        {
            description.Text = "";
            DeleteRDV_btn.IsEnabled = false;
            EditRDV_btn.IsEnabled = false;
            AddRDV_btn.IsEnabled = true;
        }
       
        private void show_image_aucun_ProchaineRDV()
        {
            if (datagrid_Prdv.Items.Count <= 0)
            {
                border_image_Aucun_RDV.Visibility = Visibility.Visible;
            }
            else border_image_Aucun_RDV.Visibility = Visibility.Collapsed;
        }

        //pour valide la date
        private bool valiadion_function_date()
        {
            Validation_RendezVous valideDate = new Validation_RendezVous(1);
            var resultDate = valideDate.Validate(Convert.ToDateTime(date_rdv.SelectedDate));

            Border? boxBorder = date_rdv.Template.FindName("BorderDate", date_rdv) as Border;
            showErrorMessage(boxBorder, RemarqueDate_rdv, resultDate);
            return resultDate.IsValid;
        }
        // function:check and show the validation message
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

        //-----------------------  END  METHODES    ------------------------------------ ---
    }
}
