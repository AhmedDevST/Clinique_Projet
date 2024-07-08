using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour edit_info_user.xaml
    /// </summary>
    public partial class edit_info_user : Window
    {
        public int ID_USER;
        public Utilisateur_Class user;
        public string filePath = string.Empty;
        public BitmapImage bitmap;
        List<KeyValuePair<int, String>> ListRole;
        List<KeyValuePair<int, String>> List_status;
        public edit_info_user(int iD_USER)
        {
            try 
            { 
                InitializeComponent();
                ID_USER = iD_USER;
                Initialiser_combobox();
                Load_user();
                Enable_Edit_User(false);    
            }
            catch (Exception)
            {
                 MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        //btn  enable edit data
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_edit.Visibility = Visibility.Collapsed;
                btn_save_data.Visibility = Visibility.Visible;
                Border_edit_photo.Visibility = Visibility.Visible;
                Enable_Edit_User(true);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //upload image
        private void Upload_photo_MouseDown(object sender, MouseButtonEventArgs e)
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
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    photo_user.ImageSource = bitmap;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //save data edit 
        private void btn_save_data_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ( checkIsValide() && Valide_login_passowrd(username.Text , password_user.Text) )
                {
                    MessageBoxResult res = MessageBox.Show("Voulez Vous Enregistrer ces informations?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        //update
                        Utilisateur_Class utilisateur_;            
                         utilisateur_ = new Utilisateur_Class(ID_USER, Nom_user.Text, prenom_user.Text, CIN_user.Text, phone_user.Text,
                                                    email_user.Text, filePath, Convert.ToInt32(combox_Activer_compte.SelectedValue), diplome_user_medcine.Text, username.Text, password_user.Text, combox_role.SelectedValue.ToString());

                         if(utilisateur_.UpdateUser())
                        {
                            new succes().ShowDialog();
                            btn_save_data.Visibility = Visibility.Collapsed;
                            Border_edit_photo.Visibility = Visibility.Collapsed;
                            btn_edit.Visibility = Visibility.Visible;
                            Enable_Edit_User(false);
                            Load_user();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("les données sont invalide");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //supprimer user
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez Vous supprimer ce utilisateur?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    //delete
                    if(Utilisateur_Class.DeleteUser(ID_USER)){
                        new succes().ShowDialog();
                        this.Close();
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // annuler btn
        private void Annuler_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Load_user();
                Enable_Edit_User(false);
                btn_edit.Visibility = Visibility.Visible;
                btn_save_data.Visibility = Visibility.Collapsed;
                Border_edit_photo.Visibility = Visibility.Collapsed;
                filePath = string.Empty;
                show_diplome();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        private void combox_role_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                show_diplome();
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

        //load user
        private void Load_user()
        {
            try
            {
                user = Utilisateur_Class.SelectUser(ID_USER);
                if (user != null)
                {
                    name_user.Text = user.nom_user + " " + user.prenom_user;
                    Nom_user.Text = user.nom_user;
                    prenom_user.Text = user.prenom_user;
                    CIN_user.Text = user.CIN_user;
                    phone_user.Text = user.phone_user;
                    email_user.Text = user.email_user;
                    diplome_user_medcine.Text = user.diplome_user;
                    username.Text = user.login_user;
                    password_user.Text = user.password_user;
                    status_user.Text = user.IsConnect;

                    //diplome
                    if (Convert.ToInt32(user.Role_user) == 1)
                    {
                        Border_diplome.Visibility = Visibility.Visible;
                        diplome_user_medcine.Text = user.diplome_user.ToString();
                    }

                    KeyValuePair<int, string> selected_Role = ListRole.FirstOrDefault(pair => pair.Key == Convert.ToInt32(user.Role_user));
                    combox_role.SelectedItem = selected_Role;
                    KeyValuePair<int, string> selected_status = List_status.FirstOrDefault(pair => pair.Key == user.Status_compte);
                    combox_Activer_compte.SelectedItem = selected_status;

                    if (user.imageData != null)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = new MemoryStream(user.imageData);
                        bitmapImage.EndInit();
                        photo_user.ImageSource = bitmapImage;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //initialiser combobox(combobox de role et de status de compte)
        private void Initialiser_combobox()
        {
            try
            {
                //list role
                ListRole = new List<KeyValuePair<int, string>>();
                ListRole.Add(new KeyValuePair<int, string>(1, "Medcine"));
                ListRole.Add(new KeyValuePair<int, string>(2, "Sécretaire"));
                combox_role.ItemsSource = ListRole;
                combox_role.DisplayMemberPath = "Value";
                combox_role.SelectedValuePath = "Key";
                combox_role.SelectedIndex = 0;

                //list status compte
                List_status = new List<KeyValuePair<int, string>>();
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

        // enable or disbale  button edit info user
        private void Enable_Edit_User(bool IsEnable)
        {
            try
            {
                //textbox
                Nom_user.IsEnabled = IsEnable;
                prenom_user.IsEnabled = IsEnable;
                CIN_user.IsEnabled = IsEnable;
                phone_user.IsEnabled = IsEnable;
                email_user.IsEnabled = IsEnable;
                diplome_user_medcine.IsEnabled = IsEnable;
                username.IsEnabled = IsEnable;
                password_user.IsEnabled = IsEnable;

                //combobox
                //interdit de deActiver le compte d Admin 
                if (user.IsAdmin == 1)
                {
                    combox_role.Visibility = Visibility.Collapsed;
                    IsAdmin_text.Visibility = Visibility.Visible;
                    combox_Activer_compte.IsEnabled = false;
                    border_delete_user.Visibility = Visibility.Collapsed;
                }
                else
                {
                    IsAdmin_text.Visibility = Visibility.Collapsed;
                    combox_role.Visibility = Visibility.Visible;
                    combox_role.IsEnabled = IsEnable;
                    combox_Activer_compte.IsEnabled = IsEnable;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        private void show_diplome()
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

        //check all inputs
        private bool checkIsValide()
        {
            try
            {
                bool v1 = Validation_function(Nom_user, RemarqueNom, 1);
                bool v2 = Validation_function(prenom_user, RemarquePrenom, 1);
                bool v3 = Validation_function(CIN_user, RemarqueCIN, 2);
                bool v4 = Validation_function(username, Remarque_username, 5);
                bool v5 = Validation_function(password_user, Remarque_password, 11);
                bool v6 = Validation_function(phone_user, RemarquePhone, 12);

                v1 = v1 && v2 && v3 && v4 && v5 && v6;
                if (email_user.Text.Length != 0)
                    v1 = v1 && Validation_function(email_user, RemarqueEmail, 3);
                return v1;
            }
            catch (Exception)
            {
                return false;
            }
        }
       
        /// return the result of validation
        private ValidationResult validation_fluent(string content, int TypeOfValidatin)
        {
            PrivatValidation valide = new PrivatValidation(TypeOfValidatin);
            ValidationResult result = valide.Validate(content);
            return result;
        }

        // check and show the validation message
        private void showErrorMessage(Border boxBorder, TextBlock Remarque, ValidationResult result)
        {
            try
            {
                if (result.IsValid == false) // si le contenu n'est pas valide
                {
                    string s = "";  //recuperer lemessage d'erreurs
                    foreach (var item in result.Errors) s += item.PropertyName + item.ErrorMessage;

                    Remarque.Text = s;
                    Remarque.Foreground =Brushes.Red;
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
            catch (Exception)
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
        private bool Valide_login_passowrd(string login ,string password)
        {
            if (Utilisateur_Class.Valide_Login_Password(ID_USER , password , login ) == 0)
            {
                return true;
            }
            return false;
        }


        //-----------------------  END  METHODES  :  ------------------------------------ ----

    }
}
