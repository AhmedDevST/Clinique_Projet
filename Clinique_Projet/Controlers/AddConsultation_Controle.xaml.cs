using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour AddConsultation_Controle.xaml
    /// </summary>
    public partial class AddConsultation_Controle : UserControl
    {
        public int Patient_ID;
        public int IsTerminer;
        public decimal poids = 0;
        public int taille = 0;
        public Utilisateur_Class user;
        ObservableCollection<string> ListPrpositions;

        public AddConsultation_Controle(int idPatient,Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Patient_ID = idPatient;
                load_patient();
                initiliser_antecdents();
                load_combobox_prpositions();
                this.user = user;     
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- SATRT EVENTS :  -------------------------------

        // btn enregistrer
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // si les donnes est valide
                if (Valide_Data())
                {
                    MessageBoxResult res = MessageBox.Show("Voullez vous ajouter cette consultation", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        // insert data in database
                        if (Save_Data())
                        {
                            // add notification
                            Notification_class notification_ =
                            new Notification_class(0, user.id_user, ConsultationClass.GetLast_Id(), "cons", DateTime.Now, " Ajouté Une nouvelle Consultation ");
                            notification_.Add_Notification();

                            new succes().ShowDialog();

                            Details_Consultation consult = new Details_Consultation(ConsultationClass.GetLast_Id(), user);
                            Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                            if (acceuil != null)
                            {
                                acceuil.ChangerControleur(consult);
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("les données ne sont pas valide");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // window antecdents
        private void deatils_antecedent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Antecdents_Window(Patient_ID, user).ShowDialog();
                initiliser_antecdents(); //load antecdents
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // validation motif
        private void motif_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(motif_patient, RemarqueMotif, 10);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //valide la taille
        private void Taille_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Taille_patient.Text.Length != 0) Validation_function(Taille_patient, RemarqueTaille, 9);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //valide poids
        private void Poids_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Poids_patient.Text.Length != 0) Validation_function(Poids_patient, RemarquePoids, 7);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
        //rechrehe les propositions de champs de consultation 
        private void combobox_propositions_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            try
            {
                string searchTerm = combobox_propositions.Text;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var filteredItems = ListPrpositions.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();

                    if (filteredItems.Count() > 0)
                    {
                        combobox_propositions.ItemsSource = filteredItems;
                        combobox_propositions.IsDropDownOpen = true;
                    }
                    else
                    {
                        combobox_propositions.IsDropDownOpen = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //img event : ajouter la proposition au champs motif 
        private void img_add_motif_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (combobox_propositions.SelectedItem != null)
                {
                    motif_patient.Text += " " + combobox_propositions.Text;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //img event : ajouter la proposition au champs diagnostique
        private void img_add_diagnostique_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (combobox_propositions.SelectedItem != null)
                {
                    Diagnostique_medical.Text += " " + combobox_propositions.Text;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //img event : ajouter la proposition au champs Examen clinique
        private void img_add_Examen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (combobox_propositions.SelectedItem != null)
                {
                    examen_clinique.Text += " " + combobox_propositions.Text;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- END EVENTS   -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //load antecdents
        private void initiliser_antecdents()
        {

            try
            {
                ObservableCollection<Gestion_Antecedent> List_Antecedents = Gestion_Antecedent.DisplayAnteced_OfPatient(Patient_ID);
                Panel_antecdents.Children.Clear();
                if (List_Antecedents.Count > 0)
                {
                    border_image_Aucun_antecedents.Visibility = Visibility.Collapsed;
                    // Generate TextBoxes and add them to the StackPanel
                    foreach (var item in List_Antecedents)
                    {
                        StackPanel panel = new StackPanel();
                        panel.Orientation = Orientation.Vertical;

                        //type antecdents
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = "Antécédent " + item.type_anteced.Nom_TypeANteced;
                        textBlock.Style = (Style)Application.Current.Resources["TextBlockStyle"];

                        // antecdents
                        TextBox textBox = new TextBox();
                        textBox.IsReadOnly = true;
                        textBox.Text = item.anteced.Descrip_Anteced;
                        textBox.Style = (Style)Application.Current.Resources["textbox_antecdent"];

                        panel.Children.Add(textBlock);
                        panel.Children.Add(textBox);
                        Panel_antecdents.Children.Add(panel);
                    }
                }
                else border_image_Aucun_antecedents.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load patient
         private void load_patient()
         {
            try 
            { 
                //nom patient
                PatientClass p = PatientClass.SelectPatient(Patient_ID);
                if (p != null)
                {
                    if (p.SexPatient == 'F')
                    {
                        NamePatient.Text = "Mm" + " " + p.NomPatient + " " + p.PrenomPatient;
                    }
                    else NamePatient.Text = "Mr" + " " + p.NomPatient + " " + p.PrenomPatient;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
         }

        //load les propositions  
        private void load_combobox_prpositions()
        {
            try
            {
                ListPrpositions = ConsultationClass.Display_champs_consultation();
                if (ListPrpositions.Count > 0)
                {
                    combobox_propositions.ItemsSource = ListPrpositions;
                    border_combobx_proposition.Visibility = Visibility.Visible;
                    img_add_diagnostique.Visibility = Visibility.Visible;
                    img_add_motif.Visibility = Visibility.Visible;
                    img_add_Examen.Visibility = Visibility.Visible;
                }
                else
                {
                    img_add_diagnostique.Visibility = Visibility.Collapsed;
                    img_add_motif.Visibility = Visibility.Collapsed;
                    img_add_Examen.Visibility = Visibility.Collapsed;
                    border_combobx_proposition.Visibility = Visibility.Collapsed;
                }
            }catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //insert data in database
        private bool Save_Data()
        {
            try
            {
                Terminer_Consult(); // donner la valeur 1 ou 0 a variable Terminer
                // add the consultation
                ConsultationClass consult = new ConsultationClass(0, motif_patient.Text, DateTime.Now, examen_clinique.Text,
                                                           Diagnostique_medical.Text, poids,
                                                           taille, IsTerminer, Patient_ID);
                consult.AddConsultation();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //valide donnes
        private bool Valide_Data()
        {

            bool bPoids = true;
            bool bTaille = true;
            bool b2 = Validation_function(motif_patient, RemarqueMotif, 10);
            //poids
            if (Poids_patient.Text.Length != 0)
            {
                if (Validation_function(Poids_patient, RemarquePoids, 7))
                {
                    poids = Convert.ToDecimal(Poids_patient.Text);
                }
                else bPoids = false;
            }
            //taille
            if (Taille_patient.Text.Length != 0)
            {
                if (Validation_function(Taille_patient, RemarqueTaille, 9))
                {
                    taille = Convert.ToInt32(Taille_patient.Text);
                }
                else bTaille = false;
            }

            return bPoids && bTaille && b2 ;
        }

        // if consult cachter ou non
        private void Terminer_Consult()
        {
            try
            {
                if (Terminer_checkBox.IsChecked == true)
                {
                    IsTerminer = 1;
                }
                else IsTerminer = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
   
        /// ---------- fonction de validation ------------

        /// function::return the result of validation
        private ValidationResult validation_fluent(string content, int TypeOfValidatin)
        {
            PrivatValidation valide = new PrivatValidation(TypeOfValidatin);
            ValidationResult result = valide.Validate(content);
            return result;
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

        // function of validation for textbox
        private bool Validation_function(TextBox MytextBox, TextBlock Remarque, int TypeOfValidation)
        {
            var result = validation_fluent(MytextBox.Text, TypeOfValidation);
            Border? boxBorder = MytextBox.Template.FindName("boxBorder", MytextBox) as Border;
            showErrorMessage(boxBorder, Remarque, result);
            return result.IsValid;
        }

        //-----------------------  END  METHODES    ------------------------------------ ----
    }
}
