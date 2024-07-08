using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Nouveau_User.xaml
    /// </summary>
    public partial class Nouveau_User : Window
    {
        public string filePath = string.Empty;

        public Nouveau_User()
        {
            try
            {
                InitializeComponent();
                Initialiser_combobox();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- SATRT EVENTS :  -------------------------------
        
        // btn save data
        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkIsValide() && Valide_login_passowrd(password_user.Text, username.Text))
                {
                    MessageBoxResult res = MessageBox.Show("Voulez Vous Enregistrer ces informations?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        //insert
                        Utilisateur_Class utilisateur_ = new Utilisateur_Class(0, Nom_user.Text, prenom_user.Text, CIN_user.Text, phone_user.Text,
                                                 email_user.Text, filePath, Convert.ToInt32(combox_Activer_compte.SelectedValue), diplome_user_medcine.Text, username.Text, password_user.Text, combox_role.SelectedValue.ToString());

                        if (utilisateur_.AddUser())
                        {
                            new succes().ShowDialog();
                            this.Close();
                            GC.Collect();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("les données sont invalide!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //btn annuler 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez Vous Annuler cette operation?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //ipload photo
        private void btn_photo_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Set initial directory if desired

                if (openFileDialog.ShowDialog() == true)
                {
                    // User selected a file, you can access the file path using openFileDialog.FileName
                    filePath = openFileDialog.FileName;
                    photo_user.Text = System.IO.Path.GetFileName(filePath);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //afficher input diplome si le role est de medcine
        private void combox_role_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if ((combox_role.SelectedItem != null) && (Convert.ToInt32(combox_role.SelectedValue) == 1))
                {
                    Border_diplome.Visibility = Visibility.Visible;
                }
                else Border_diplome.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //valide nom de utilisateur
        private void Nom_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(Nom_user, RemarqueNom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide prenom de utilisateur
        private void prenom_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(prenom_user, RemarquePrenom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide CIN de utilisateur
        private void CIN_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(CIN_user, RemarqueCIN, 2);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide phone de utilisateur
        private void phone_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(phone_user, RemarquePhone, 12);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide email de utilisateur
        private void email_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (email_user.Text.Length != 0) Validation_function(email_user, RemarqueEmail, 3);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide username de utilisateur
        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(username, Remarque_username, 5);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //valide mot de passe de utilisateur
        private void password_user_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(password_user, Remarque_password, 11);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- END EVENTS :  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void Initialiser_combobox()
        {
            try
            {
                //list role
                List<KeyValuePair<int, string>> ListRole = new List<KeyValuePair<int, string>>();
                ListRole.Add(new KeyValuePair<int, string>(2, "Sécretaire"));
                ListRole.Add(new KeyValuePair<int, string>(1, "Medcine"));
                combox_role.ItemsSource = ListRole;
                combox_role.DisplayMemberPath = "Value";
                combox_role.SelectedValuePath = "Key";
                combox_role.SelectedIndex = 0;

                //list status compte
                List<KeyValuePair<int, string>> List_status = new List<KeyValuePair<int, string>>();
                List_status.Add(new KeyValuePair<int, string>(0, " Désactive"));
                List_status.Add(new KeyValuePair<int, string>(1, "Active"));
                combox_Activer_compte.ItemsSource = List_status;
                combox_Activer_compte.DisplayMemberPath = "Value";
                combox_Activer_compte.SelectedValuePath = "Key";
                combox_Activer_compte.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //function:check all inputs
        private bool checkIsValide()
        {
            try
            {
                bool v1 = Validation_function(Nom_user, RemarqueNom, 1);
                bool v2 = Validation_function(prenom_user, RemarquePrenom, 1);
                bool v3 = Validation_function(CIN_user, RemarqueCIN, 6);
                bool v4 = Validation_function(username, Remarque_username, 5);
                bool v5 = Validation_function(password_user, Remarque_password, 11);
                bool v6 = Validation_function(phone_user, RemarquePhone, 12);

                v1 = v1 && v2 && v3 && v4 && v5 && v6;
                if (email_user.Text.Length != 0) v1 = v1 && Validation_function(email_user, RemarqueEmail, 3);
                return v1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite");
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
        //valider le password
        private bool Valide_login_passowrd(string password , string  login)
        {
            if (Utilisateur_Class.Valide_Login_Password(-1 , password, login ) == 0)
            {
                return true;
            }
            return false;
        }

        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
