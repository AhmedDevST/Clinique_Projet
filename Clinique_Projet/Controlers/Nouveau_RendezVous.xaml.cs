using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Nouveau_RendezVous.xaml
    /// </summary>   

    
    public partial class Nouveau_RendezVous : UserControl
    {
        List<KeyValuePair<int, string>> ListPatient = PatientClass.ListPatient();
        Utilisateur_Class user;
        public Nouveau_RendezVous(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                affecter_date_now();
                blocker_date();
                //annuler.Click += Button_Click;    

            }
            catch (Exception) 
            { 
                MessageBox.Show("c'est un dimanche n'est ce pas!!!");
            }
        }
        // -------------------------- SATRT EVENTS :  -------------------------------

        // btn quitter 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Voulez Vous quitter!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    home liste_rendezVous = new home(user);
                    Acceuil? acceuil = Window.GetWindow(this) as Acceuil;
                    if (acceuil != null)
                    {
                        acceuil.ChangerControleur(liste_rendezVous);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // btn ajouter
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                if (combobox_patient.Text == "" || !valiadion_function_date())
                {
                    MessageBox.Show("Données Incompletes!!");
                    if (combobox_patient.Text == "")
                    {
                        RemarqueNom.Text = "Veuillez choisir un patient";
                        RemarqueNom.Foreground = Brushes.Red;
                    }
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

        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {
                DateTime time = (DateTime)picker.SelectedTime;
                real.Text = time.Hour.ToString() + " : " + time.Minute.ToString();

                // faire un test sur la validitée de l'heure choisie ==> si cette heure est déja reservé ou s'elle respecte le temps du travail.
                if (time.Hour < 8 || time.Hour > 20 ||
                        !RendezVous.date_heure_valide(picker.SelectedTime.Value.ToLongTimeString(), dateNaissance_patient.SelectedDate.Value))
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
                MessageBox.Show("Opération d'entrée innatendu N M210 !!");
            }
        }
        
        // btn : ajouter un patient 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Nouveau_Patient nouveau_Patient = new Nouveau_Patient(user);
                nouveau_Patient.ShowDialog();
                ListPatient = PatientClass.ListPatient();
                combobox_patient.Text = ListPatient[ListPatient.Count - 1].ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatenduN M211 !!");
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
        private void Nom_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validation_function(combobox_patient, RemarqueNom, 1);
        }
        private void combobox_patient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                string searchTerm = combobox_patient.Text;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    //condition de serach
                    var filteredItems = ListPatient.Where(item => item.Value.ToLower().Contains(searchTerm.ToLower())).ToList();

                    if (filteredItems.Count() > 0) // si ona des results
                    {
                        combobox_patient.ItemsSource = filteredItems;

                        combobox_patient.IsDropDownOpen = true;  //affiche  dropDown de combobox
                    }
                    else //sinon on n'affiche pas dropDown de combobox
                    {
                        combobox_patient.IsDropDownOpen = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void combobox_patient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RemarqueNom.Text = "Champs Validé";
                RemarqueNom.Foreground = Brushes.Green;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void combobox_patient_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (combobox_patient.Text == "")
                {
                    RemarqueNom.Text = "Champs invalide";
                    RemarqueNom.Foreground = Brushes.Red;
                }
                else
                {
                    RemarqueNom.Text = "champs valide";
                    RemarqueNom.Foreground = Brushes.Green;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void affecter_date_now()
        {
            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                dateNaissance_patient.SelectedDate = DateTime.Now;
            else
                dateNaissance_patient.SelectedDate = DateTime.Now.AddDays(1);
        }
        private void blocker_date()
        {
            // Récupérer la date du système
            DateTime systemDate = DateTime.Now;

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
        private void validation(string heure_rendezVous)
        {
            if (!RendezVous.date_heure_valide(heure_rendezVous, dateNaissance_patient.SelectedDate.Value))
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
                        dateNaissance_patient.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
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

                        Acceuil? acceuil = Window.GetWindow(this) as Acceuil;
                        if (acceuil != null)
                        {

                            RendezVous.ajouter_RendezVous(Convert.ToDateTime(dateNaissance_patient.SelectedDate), heure_rendezVous,
                                comboxType_Certificate.Text.ToString(), combobox_patient.Text.ToString(), description.Text);
                            // add notification
                            Notification_class notification_ =
                            new Notification_class(0, user.id_user, RendezVous.GetLast_Id(), "rdv", DateTime.Now, " Ajouté Un nouveau Rendez Vous ");
                            notification_.Add_Notification();

                            // Affichage du message de succés!
                            home liste_rendezVous = new home(user);
                            acceuil.ChangerControleur(liste_rendezVous);
                            new succes().ShowDialog();
                        }
                    }
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
        private bool Validation_function(TextBox MytextBox, TextBlock Remarque, int TypeOfValidation)
        {
            var result = validation_fluent(MytextBox.Text, TypeOfValidation);
            Border? boxBorder = MytextBox.Template.FindName("boxBorder", MytextBox) as Border;
            showErrorMessage(boxBorder, Remarque, result);
            return result.IsValid;
        }
        private ValidationResult validation_fluent(string content, int TypeOfValidatin)
        {
            PrivatValidation valide = new PrivatValidation(TypeOfValidatin);
            ValidationResult result = valide.Validate(content);
            return result;
        }
         private bool valiadion_function_date()
        {
            Validation_RendezVous valideDate = new Validation_RendezVous(1);
            var resultDate = valideDate.Validate(Convert.ToDateTime(dateNaissance_patient.SelectedDate));
            
            Border? boxBorder = dateNaissance_patient.Template.FindName("BorderDate", dateNaissance_patient) as Border;
            showErrorMessage(boxBorder, RemarqueDateNaiss, resultDate);
            return resultDate.IsValid;
        }

        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
