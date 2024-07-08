using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Nouveau_Patient.xaml
    /// </summary>
    public partial class Nouveau_Patient : Window
    {
        public static string BackColor = "#000000";

        public Utilisateur_Class user;

        //list va contenient tous les anteceddents  
        ObservableCollection<Gestion_Antecedent> List_Antecedents = new ObservableCollection<Gestion_Antecedent>();
       
        public Gestion_Antecedent? ObjAnteced = null; //cette objet pour affichage antecednet
        public Nouveau_Patient(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                Droits_user();
                loadCombobx();
                Initiliser_btn_antecdent();
            }
            catch (Exception)
            {
                 MessageBox.Show("Opération d'entrée innatendu N M207 !!");
            }  
        }



        // -------------------------- SATRT EVENTS :  -------------------------------

        // btn save info
        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkIsValide() == true)
                {
                    Age_Patient.Text = (DateTime.Now.Year - dateNaissance_patient.SelectedDate.Value.Year).ToString();
                    MessageBoxResult res = MessageBox.Show("Voulez Vous Enregistrer ces informations?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        //Ajouter dans base de donnee

                        PatientClass patient = new PatientClass(0, Nom_patient.Text, prenom_Patient.Text, Convert.ToDateTime(dateNaissance_patient.SelectedDate),
                                          CIN_patient.Text, Convert.ToChar(comboxSex.SelectedValue), DateTime.Now, Convert.ToInt32(Age_Patient.Text), lieuNaiss_Patient.Text,
                                          phone_patient.Text, email_patient.Text, address_Patient.Text, Proffesion_Patient.Text,
                                      comboboxAssurane.SelectedValue.ToString(), comboboxSang.SelectedValue.ToString(), comboboxCivil.Text);
                        if (patient.AddPatient())
                        {
                            // si user est un docteur :peut gerer les antecdent
                            if (Convert.ToInt32(user.Role_user) == 1)
                            {
                                // Ajouter dans base de donnee
                                Gestion_Antecedent.Add_AllAntecedents(PatientClass.GetLast_Id(), List_Antecedents);
                            }
                            // add notification
                            Notification_class notification_ =
                            new Notification_class(0, user.id_user, PatientClass.GetLast_Id(), "pat", DateTime.Now, " Ajouté Un nouveau patient ");
                            notification_.Add_Notification();
                            new succes().ShowDialog();
                            this.Close();
                            GC.Collect();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu N M112 !!");
                    }
                }
                else MessageBox.Show("les données sont invalide");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M113 !!");
            }
        }
        // annuler btn
        private void annuler_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultat = MessageBox.Show("Êtes-vous sûr de vouloir quitter ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultat == MessageBoxResult.Yes)
                    Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M205 !!");
            }
        }

        //Nom de patient
        private void Nom_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(Nom_patient, RemarqueNom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M200 !!");
            }
        }
        //prenom de patient
        private void prenom_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(prenom_Patient, RemarquePrenom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M201 !!");
            }
        }
        //CIN de patient
        private void CIN_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CIN_patient.Text.Length != 0) Validation_function(CIN_patient, RemarqueCIN, 6);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M203 !!");
            }
        }
        // Date patient
        private void dateNaissance_patient_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (valiadion_function_date())
                    Age_Patient.Text = (DateTime.Now.Year - dateNaissance_patient.SelectedDate.Value.Year).ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M204 !!");
            }
        }
        //email de patient
        private void email_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (email_patient.Text.Length != 0) Validation_function(email_patient, RemarqueEmail, 3);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //phone de patient
        private void phone_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (phone_patient.Text.Length != 0) Validation_function(phone_patient, RemarquePhone, 4);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //lieu de naissance de patient
        private void lieuNaiss_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lieuNaiss_Patient.Text.Length != 0) Validation_function(lieuNaiss_Patient, RemarqueLieuNaiss, 5);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //address de patient
        private void address_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (address_Patient.Text.Length != 0) Validation_function(address_Patient, RemarqueAdresse, 5);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //proffesion de patient
        private void Proffesion_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Proffesion_Patient.Text.Length != 0) Validation_function(Proffesion_Patient, RemarqueProffesion, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }  
        // prametre asssurance
        private void parametre_assurance_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Parametres("assurance", user).ShowDialog();
                load_combobox_assurance();
                comboboxAssurane.SelectedIndex = comboboxAssurane.Items.Count;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //prametre group sang
        private void parametre_groupsang_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Parametres("group_sang", user).ShowDialog();
                load_combobox_groupsang();
                comboboxSang.SelectedIndex = comboboxSang.Items.Count;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        //---------------Antecedents --------------------
        
        //add antecedents
        private void AddAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_TypeAnteced;
                if (comboboxType_Anteced.SelectedItem != null)
                {
                    id_TypeAnteced = Convert.ToInt32(comboboxType_Anteced.SelectedValue.ToString());

                    if (!List_Antecedents.Any(inteced => inteced.anteced.IDType_Anteced == id_TypeAnteced))
                    {
                        // confirmation
                        MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Antécedent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            List_Antecedents.Add(new Gestion_Antecedent
                            {
                                anteced = new Antecedents { IDType_Anteced = id_TypeAnteced, Date_Anteced = DateTime.Now, Descrip_Anteced = Description_Anteced.Text },
                                type_anteced = new TypeAntecedent { Nom_TypeANteced = comboboxType_Anteced.Text },
                            });

                            datagrid_Antecedent.ItemsSource = null;
                            datagrid_Antecedent.ItemsSource = List_Antecedents;
                            show_image_aucun_antecdents();
                            Annuler_OperartionAntecedant();
                        }
                    }
                    else MessageBox.Show("deja existe");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //show anteced
        private void show_Anteced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Antecedent.SelectedItem != null)
                {
                    ObjAnteced = (Gestion_Antecedent)datagrid_Antecedent.SelectedItem;
                    comboboxType_Anteced.Text = ObjAnteced.type_anteced.Nom_TypeANteced;
                    comboboxType_Anteced.IsEnabled = false;
                    Description_Anteced.Text = ObjAnteced.anteced.Descrip_Anteced;
                    DeleteAnteced_btn.IsEnabled = true;
                    EditAnteced_btn.IsEnabled = true;
                    AddAnteced_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //edit antecedents
        private void EditAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // confirmation
                MessageBoxResult res = MessageBox.Show("Voullez vous modifier cet Antécedent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjAnteced != null)
                    {
                        foreach (var item in List_Antecedents)
                        {
                            if (item.anteced.IDType_Anteced == ObjAnteced.anteced.IDType_Anteced)
                            {
                                item.anteced.Descrip_Anteced = Description_Anteced.Text;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(" l'Opértion est annulée ");
                    }
                    Annuler_OperartionAntecedant();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //delete antecedents
        private void DeleteAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de supprimer cet Antécedent  ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjAnteced != null)
                    {
                        foreach (var item in List_Antecedents)
                        {
                            if (item.anteced.IDType_Anteced == ObjAnteced.anteced.IDType_Anteced)
                            {
                                List_Antecedents.Remove(item);
                                Annuler_OperartionAntecedant();
                                break;
                            }
                        }
                        show_image_aucun_antecdents();
                    }
                    else
                    {
                        MessageBox.Show(" l'Opértion est annulée ");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }   
        //annuler antecedents
        private void AnnulerAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Annuler_OperartionAntecedant();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //parametre antecedents
        private void add_antecedent_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Parametres("antecedent", user).ShowDialog();
                loadType_Antecedent();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS :  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //chek role
        private void Droits_user()
        {
            try { 
                switch (Convert.ToInt32(user.Role_user))
                {
                    case 1: Antecdent_item.Visibility = Visibility.Visible;
                            parametre_assurance_img.Visibility = Visibility.Visible;
                            parametre_groupsang_img.Visibility = Visibility.Visible;
                            loadType_Antecedent();
                            break;
                             // docteur

                    case 2: Antecdent_item.Visibility = Visibility.Collapsed;
                             parametre_assurance_img.Visibility = Visibility.Collapsed;
                             parametre_groupsang_img.Visibility = Visibility.Collapsed;
                        break; // secreatire

                    default:
                        Antecdent_item.Visibility = Visibility.Collapsed;
                        parametre_assurance_img.Visibility = Visibility.Collapsed;
                        parametre_groupsang_img.Visibility = Visibility.Collapsed;
                        break;
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Opération d'entrée innatendu N M208 !!");
            }
        } 
        //list assurance
        private void load_combobox_assurance()
        {
            try 
            { 
                comboboxAssurane.ItemsSource = AssuranceClass.DisplayAssurance();
                comboboxAssurane.DisplayMemberPath = "Value";
                comboboxAssurane.SelectedValuePath = "Key";
                comboboxAssurane.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M209 !!");
            }
        }
        //list etat civil
        private void load_combobox_etat_civil()
        {
            try 
            { 
                List<KeyValuePair<int, string>> ListEtatCivil = new List<KeyValuePair<int, string>>();
                ListEtatCivil.Add(new KeyValuePair<int, string>(0, "aucun"));
                ListEtatCivil.Add(new KeyValuePair<int, string>(1, "celebataire"));
                ListEtatCivil.Add(new KeyValuePair<int, string>(2, "couple"));
                ListEtatCivil.Add(new KeyValuePair<int, string>(3, "devoure"));
                ListEtatCivil.Add(new KeyValuePair<int, string>(4, "Autre"));
                comboboxCivil.ItemsSource = ListEtatCivil;
                comboboxCivil.DisplayMemberPath = "Value";
                comboboxCivil.SelectedValuePath = "Key";
                comboboxCivil.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //list sex
        private void load_combobox_Sex()
        {
            try 
            { 
                List<KeyValuePair<char, string>> ListSex = new List<KeyValuePair<char, string>>();
                ListSex.Add(new KeyValuePair<char, string>('H', "Homme"));
                ListSex.Add(new KeyValuePair<char, string>('F', "Femme"));
                comboxSex.ItemsSource = ListSex;
                comboxSex.DisplayMemberPath = "Value";
                comboxSex.SelectedValuePath = "Key";
                comboxSex.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //list groupsang  
        private void load_combobox_groupsang()
        {
            try 
            { 
                comboboxSang.ItemsSource = GroupSangClass.DisplayGroupSang();
                comboboxSang.DisplayMemberPath = "Value";
                comboboxSang.SelectedValuePath = "Key";
                comboboxSang.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //load combobox
        private void loadCombobx()
        {
            try 
            { 
                load_combobox_assurance();
                load_combobox_etat_civil();
                load_combobox_groupsang();
                load_combobox_Sex();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //---------------Antecedents-------------------
       
        //Initiliser_btn_antecdent
        private void Initiliser_btn_antecdent()
        {
            DeleteAnteced_btn.IsEnabled = false;
            EditAnteced_btn.IsEnabled = false;
        }
        private void Annuler_OperartionAntecedant()
        {
            try
            {
                ObjAnteced = null;
                Description_Anteced.Text = "";
                datagrid_Antecedent.SelectedIndex = -1;
                comboboxType_Anteced.IsEnabled = true;
                comboboxType_Anteced.SelectedIndex = 0;
                EditAnteced_btn.IsEnabled = false;
                DeleteAnteced_btn.IsEnabled = false;
                AddAnteced_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //load_type_Antecedents
        private void loadType_Antecedent()
        {
            try
            {
                comboboxType_Anteced.ItemsSource = TypeAntecedent.DisplayTypeAnteced();
                comboboxType_Anteced.SelectedValuePath = "ID_TypeANteced";
                comboboxType_Anteced.DisplayMemberPath = "Nom_TypeANteced";
                comboboxType_Anteced.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void show_image_aucun_antecdents()
        {
            try
            {
                if (List_Antecedents.Count <= 0)
                {
                    border_image_Aucun_antecedents.Visibility = Visibility.Visible;
                }
                else border_image_Aucun_antecedents.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        //---------------validation-------------------
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

        //  function of validation for  date
        private bool valiadion_function_date()
        {
            Validation_Date valideDate = new Validation_Date(1);
            var resultDate = valideDate.Validate(Convert.ToDateTime(dateNaissance_patient.SelectedDate));

            Border? boxBorder = dateNaissance_patient.Template.FindName("BorderDate", dateNaissance_patient) as Border;
            showErrorMessage(boxBorder,RemarqueDateNaiss, resultDate);
            return resultDate.IsValid;
        }
     
        //function:check all inputs
        private bool checkIsValide()
        {
            bool v1 = Validation_function(Nom_patient, RemarqueNom, 1);
            bool v2= Validation_function(prenom_Patient, RemarquePrenom, 1);
            bool v3 = valiadion_function_date();
            v1 = v1 && v2 && v3;
            if (CIN_patient.Text.Length != 0) v1=v1 && Validation_function(CIN_patient, RemarqueCIN, 6);
            if (Proffesion_Patient.Text.Length != 0) v1 = v1 && Validation_function(Proffesion_Patient,RemarqueProffesion, 1);
             if (lieuNaiss_Patient.Text.Length != 0) v1 = v1 && Validation_function(lieuNaiss_Patient,RemarqueLieuNaiss, 5);
             if (phone_patient.Text.Length != 0) v1 = v1 && Validation_function(phone_patient,RemarquePhone, 4);
             if (email_patient.Text.Length != 0) v1 = v1 && Validation_function(email_patient,RemarqueEmail, 3);
             if (address_Patient.Text.Length != 0) v1 = v1 && Validation_function(address_Patient,RemarqueAdresse, 5);
             
            return v1;  
        }

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
