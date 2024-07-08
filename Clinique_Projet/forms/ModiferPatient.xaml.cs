using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour ModiferPatient.xaml
    /// </summary>
    public partial class ModiferPatient : Window
    {
        public static int idPatient;
        public Utilisateur_Class user;
        private string CIN = null;
        List<KeyValuePair<int, String>> ListEtatCivil;
        List<KeyValuePair<char, String>> ListSex;
        List<KeyValuePair<int, String>> ListAssurance;
        List<KeyValuePair<int, String>> ListGroupSnag;

        //antecdents
        ObservableCollection<Gestion_Antecedent> List_Antecedents = new ObservableCollection<Gestion_Antecedent>();
      
        public Gestion_Antecedent? ObjAnteced = null; //cette objet pour affichage antecednet

        public SqlTableDependency<DMPatient> PatientSqlTableDependency;

        public SqlTableDependency<DMAntecedents> AntecedentsSqlTableDependency;
        public ModiferPatient(int id,Utilisateur_Class user)
        {
            try
            { 
                InitializeComponent();
                idPatient = id;
                this.user=user;
                loadCombobx();  
                Droits_user();
                loadType_Antecedent();
                LoadData_Antecedents();
                loadPatinet();
                DeleteAnteced_btn.IsEnabled = false;
                EditAnteced_btn.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- SATRT EVENTS :  -------------------------------
        
        // btn save
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
                        //constructor 
                        PatientClass patient = new PatientClass(idPatient, Nom_patient.Text, prenom_Patient.Text, Convert.ToDateTime(dateNaissance_patient.SelectedDate),
                                          CIN_patient.Text, Convert.ToChar(comboxSex.SelectedValue), DateTime.Now, Convert.ToInt32(Age_Patient.Text), lieuNaiss_Patient.Text,
                                          phone_patient.Text, email_patient.Text, address_Patient.Text, Proffesion_Patient.Text,
                                      comboboxAssurane.SelectedValue.ToString(), comboboxSang.SelectedValue.ToString(), comboboxCivil.Text);

                        //  update les donnes
                        if (patient.UpdatePatient())
                        {
                            // add notification
                            Notification_class notification_ =
                            new Notification_class(0, user.id_user, idPatient, "pat", DateTime.Now, " Modifié Un  patient ");
                            notification_.Add_Notification();
                            this.Close();
                            new succes().ShowDialog();
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

        // btn annuler
        private void annuler_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez vous quitter", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                    this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //close  window 
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    Task.Run(() => Stop_TablePatient_Dependency()),
                    Task.Run( () => Stop_TableAntecedents_Dependency())
                );
            }
            catch (Exception)
            {
            }
        }

        //open  window 
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => Start_TablePatient_Dependency());     
                // si on docteur on effectuer SqlTableDependency on Antecedents
                if (Convert.ToInt32(user.Role_user) == 1)
                    await Task.Run(() => Start_TableAntecedents_Dependency());
            }
            catch (Exception)
            {
            }
        }

        // -----------------events  patient ---------------------- 
        //nom patient
        private void Nom_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(Nom_patient, RemarqueNom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //prenom
        private void prenom_Patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Validation_function(prenom_Patient, RemarquePrenom, 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //cin
        private void CIN_patient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (CIN_patient.Text.Length != 0) valideCIN();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //date
        private void dateNaissance_patient_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (valiadion_function_date())
                    Age_Patient.Text = (DateTime.Now.Year - dateNaissance_patient.SelectedDate.Value.Year).ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //lieu Niassance
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

        //phone
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

        //email
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

        //address
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

        //profession
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
      
        //prametre assurance
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


        // -----------------events  antecedents---------------------- 
        //add antecedents
        private void AddAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_TypeAnteced;
                if (comboboxType_Anteced.SelectedItem != null)
                {
                    id_TypeAnteced = Convert.ToInt32(comboboxType_Anteced.SelectedValue.ToString());

                    // tester si n est pas deja existe
                    if (!List_Antecedents.Any(inteced => inteced.anteced.IDType_Anteced == id_TypeAnteced))
                    {
                        // confirmation
                        MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Antécedent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            // Ajouter dans base de donnee
                            Antecedents Anteced = new Antecedents(idPatient, id_TypeAnteced, DateTime.Now, Description_Anteced.Text);
                            if (Anteced.AddAntecedent())
                            {
                                MessageBox.Show("antecdent est bien ajouter ");
                                //sql dependecy va modifier dans la list des Antecedents 
                            }
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

                    //remplir les champs
                    comboboxType_Anteced.Text = ObjAnteced.type_anteced.Nom_TypeANteced;
                    Description_Anteced.Text = ObjAnteced.anteced.Descrip_Anteced;

                    //disable combobox
                    comboboxType_Anteced.IsEnabled = false;

                    //enable edit and delete btn
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
                        //modifier dans la base de donnee
                        Antecedents Anteced = new Antecedents(idPatient, ObjAnteced.anteced.IDType_Anteced, Description_Anteced.Text);
                        if (Anteced.UpdateAntecedent())
                        {
                            MessageBox.Show("antecdent est bien modifier ");
                            //sql dependecy va modifier dans la list des Antecedents 
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
                        //delete dans la base de donnee
                        if (Antecedents.DeleteAntecedent(idPatient, ObjAnteced.anteced.IDType_Anteced))
                        {
                            MessageBox.Show("antecdents est bien supprimer ");
                            //sql dependecy va actualiser le datagrid 
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


        // -------------------------- END EVENTS :  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

         // ----------- start sql dependecy :Patient  -------------
        private void Start_TablePatient_Dependency()
        {
            try
            {
                if (PatientSqlTableDependency == null)
                {
                    PatientSqlTableDependency = new SqlTableDependency<DMPatient>(ConnectDb.GetConnectionstring(), "Patient");
                    PatientSqlTableDependency.OnChanged += Changed_Patient_Dependency;
                    PatientSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {
            }
        }
        private void Changed_Patient_Dependency(object sender, RecordChangedEventArgs<DMPatient> e)
        {
            try
            {
                DMPatient EntityPatient = e.Entity;
                // si on des modification dans patient sauf le cas de insertion
                if ( (e.ChangeType != ChangeType.None) && (e.ChangeType != ChangeType.Insert) )
                {
                    if (EntityPatient != null)
                    {
                        // si la modification fait sur ce patient 
                        if (EntityPatient.id_patient == idPatient)
                        {
                            switch (e.ChangeType)
                            {
                                case ChangeType.Update:
                                    Dispatcher.Invoke(() =>
                                    {
                                        new Notification_window
                                                        (" le  patient " + EntityPatient.Nom_patient + " " + EntityPatient.Prenom_patient + " a ete  modifier par un utilisateur ")
                                                        .ShowDialog();
                                        loadPatinet();
                                    });
                                    break;

                                case ChangeType.Delete:
                                    Dispatcher.Invoke(() =>
                                    {
                                        new Notification_window
                                                        (" le  patient " + EntityPatient.Nom_patient + " " + EntityPatient.Prenom_patient + " a ete  supprimer par un utilisateur")
                                                        .ShowDialog();
                                        this.Close();
                                    });
                                    break;
                            }
                        }
                    }
                }  
            }
            catch (Exception)
            {}
        }
        private void Stop_TablePatient_Dependency()
        {
            try
            {
                PatientSqlTableDependency?.Stop();
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy : Patient -------------

        // ----------- start sql dependecy :Antecdents  -------------
        private void Start_TableAntecedents_Dependency()
        {
            try
            {
                if (AntecedentsSqlTableDependency == null)
                {
                    AntecedentsSqlTableDependency = new SqlTableDependency<DMAntecedents>(ConnectDb.GetConnectionstring(), "Antecedents");
                    AntecedentsSqlTableDependency.OnChanged += Changed_Antecedents_Dependency;
                    AntecedentsSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Antecedents_Dependency(object sender, RecordChangedEventArgs<DMAntecedents> e)
        {
            try
            {
                DMAntecedents EntityAntecedents = e.Entity;
                if (EntityAntecedents != null)
                {
                    if (EntityAntecedents.patient_id == idPatient)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                    List_Antecedents.Add(new Gestion_Antecedent
                                    {
                                        anteced = new Antecedents { IDType_Anteced = EntityAntecedents.TypeAtecd_id, Date_Anteced = EntityAntecedents.Date_Anteced, Descrip_Anteced = EntityAntecedents.Descrip_Antecedent },
                                        type_anteced = new TypeAntecedent { Nom_TypeANteced = TypeAntecedent.SelectNAme_TypeAnteced(EntityAntecedents.TypeAtecd_id) },
                                    });

                                    datagrid_Antecedent.ItemsSource = null;
                                    datagrid_Antecedent.ItemsSource = List_Antecedents;
                                    show_image_aucun_antecdents();

                                    if (ObjAnteced != null)
                                    {
                                        SelectionItemAntecedant(ObjAnteced.anteced.IDType_Anteced);
                                    }
                                });
                                break;

                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Antecedents)
                                    {
                                        if (item.anteced.IDType_Anteced == EntityAntecedents.TypeAtecd_id)
                                        {
                                            item.anteced.Descrip_Anteced = EntityAntecedents.Descrip_Antecedent;
                                            // si le bilans qui modifier etait selectionner
                                            if ((ObjAnteced != null) && (ObjAnteced.anteced.IDType_Anteced == EntityAntecedents.TypeAtecd_id))
                                                Description_Anteced.Text = EntityAntecedents.Descrip_Antecedent;
                                            return;
                                        }
                                    }
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Antecedents)
                                    {
                                        if (item.anteced.IDType_Anteced == EntityAntecedents.TypeAtecd_id)
                                        {
                                            List_Antecedents.Remove(item);
                                            break;
                                        }
                                    }

                                    datagrid_Antecedent.ItemsSource = null;
                                    datagrid_Antecedent.ItemsSource = List_Antecedents;
                                    show_image_aucun_antecdents();

                                    if (ObjAnteced != null)
                                    {
                                        // si on supprimer Antecedant qui ete selectionner par utilisateur
                                        if (ObjAnteced.anteced.IDType_Anteced == EntityAntecedents.TypeAtecd_id)
                                        {
                                            Annuler_OperartionAntecedant();
                                        }
                                        else
                                        {
                                            SelectionItemAntecedant(ObjAnteced.anteced.IDType_Anteced);
                                        }
                                    }

                                });
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void Stop_TableAntecedents_Dependency()
        {
            try
            {
                AntecedentsSqlTableDependency?.Stop();
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy :Antecdents  -------------

        // ------------methodes Patient--------------------
        //chek role user
        private void Droits_user()
            {
                try
                {
                    switch (Convert.ToInt32(user.Role_user))
                    {
                        case 1:
                            Antecdent_item.Visibility = Visibility.Visible;
                            parametre_assurance_img.Visibility = Visibility.Visible;
                            parametre_groupsang_img.Visibility = Visibility.Visible;
                            loadType_Antecedent();
                            break;
                        // docteur

                        case 2:
                            Antecdent_item.Visibility = Visibility.Collapsed;
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
                catch (Exception)
                {
                    MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
        //list assurance
        private void load_combobox_assurance()
        {
            try
            {
                ListAssurance = AssuranceClass.DisplayAssurance(); ;
                comboboxAssurane.ItemsSource = AssuranceClass.DisplayAssurance();
                comboboxAssurane.DisplayMemberPath = "Value";
                comboboxAssurane.SelectedValuePath = "Key";
                comboboxAssurane.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //list etat civil
        private void load_combobox_etat_civil()
        {
            try
            {
                ListEtatCivil = new List<KeyValuePair<int, string>>();
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
                 ListSex = new List<KeyValuePair<char, string>>();
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
                ListGroupSnag= GroupSangClass.DisplayGroupSang(); 
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
        // load information of patient
        private void loadPatinet()
        {
            try
            { 
                PatientClass p = PatientClass.SelectPatient(idPatient);
                if (p != null)
                {
                    FullName_patient.Text = "";
                    FullName_patient.Text += p.NomPatient + " " + p.PrenomPatient;
                    Nom_patient.Text = p.NomPatient;
                    prenom_Patient.Text = p.PrenomPatient;
                    CIN_patient.Text = p.CINPatient;
                    CIN = p.CINPatient;
                    dateNaissance_patient.SelectedDate = p.DateNaissancePatient;
                    lieuNaiss_Patient.Text = p.LieuNaissancePatient;
                    Age_Patient.Text = p.AgePatient.ToString();
                    phone_patient.Text = p.PhonePatient;
                    email_patient.Text = p.EmailPatient;
                    address_Patient.Text = p.AddressPatient;
                    Proffesion_Patient.Text = p.ProfessPatient;

                    KeyValuePair<int, string> selectedPairCIvil = ListEtatCivil.FirstOrDefault(pair => pair.Value == p.EtatCivilPatient);
                    comboboxCivil.SelectedItem = selectedPairCIvil;
                    KeyValuePair<int, string> selectedPairAssurance = ListAssurance.FirstOrDefault(pair => pair.Key == Convert.ToInt32(p.AssurancePatient));
                    comboboxAssurane.SelectedItem = selectedPairAssurance;
                    KeyValuePair<int, string> selectedPairSang = ListGroupSnag.FirstOrDefault(pair => pair.Key == Convert.ToInt32(p.GroupSangPatient));
                    comboboxSang.SelectedItem = selectedPairSang;
                    KeyValuePair<char, string> selectedPairSex = ListSex.FirstOrDefault(pair => pair.Key == p.SexPatient);
                    comboxSex.SelectedItem = selectedPairSex;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // --------------methodes antecdents------------------

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
        //load data of Antecedents
        private void LoadData_Antecedents()
        {
            try
            {
                List_Antecedents = Gestion_Antecedent.DisplayAnteced_OfPatient(idPatient);
                datagrid_Antecedent.ItemsSource = List_Antecedents;
                show_image_aucun_antecdents();
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

        // iniasilaiser les champs 
        private void Annuler_OperartionAntecedant()
        {
            try
            {
                ObjAnteced = null;
                datagrid_Antecedent.SelectedIndex = -1;
                Description_Anteced.Text = "";
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
      
        //selectuionner par index 
        private void SelectionItemAntecedant(int Id)
        {
            foreach (Gestion_Antecedent item in datagrid_Antecedent.Items)
            {
                if (item.anteced.IDType_Anteced == Id)
                {
                    datagrid_Antecedent.SelectedIndex = datagrid_Antecedent.Items.IndexOf(item);
                    break;
                }
            }
        }


        // --------------methode de vlidation---------------

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
            showErrorMessage(boxBorder, RemarqueDateNaiss, resultDate);
            return resultDate.IsValid;
        }

        //function:check all inputs
        private bool checkIsValide()
        {
            bool v1 = Validation_function(Nom_patient, RemarqueNom, 1);
            bool v2 = Validation_function(prenom_Patient, RemarquePrenom, 1);
            bool v3 = valiadion_function_date();
            v1 = v1 && v2 && v3 ;
            if (CIN_patient.Text.Length != 0) v1 = v1 && valideCIN();
            if (Proffesion_Patient.Text.Length != 0) v1 = v1 && Validation_function(Proffesion_Patient, RemarqueProffesion, 1);
            if (lieuNaiss_Patient.Text.Length != 0) v1 = v1 && Validation_function(lieuNaiss_Patient, RemarqueLieuNaiss, 5);
            if (phone_patient.Text.Length != 0) v1 = v1 && Validation_function(phone_patient, RemarquePhone, 4);
            if (email_patient.Text.Length != 0) v1 = v1 && Validation_function(email_patient, RemarqueEmail, 3);
            if (address_Patient.Text.Length != 0) v1 = v1 && Validation_function(address_Patient, RemarqueAdresse, 5);

            return v1;

        }
        //valide cin
        private bool valideCIN()
        {
            if (CIN != null)
            {
                if ((CIN.ToUpper() != CIN_patient.Text.ToUpper()))
                    return Validation_function(CIN_patient, RemarqueCIN, 6); //test unique cin

                return Validation_function(CIN_patient, RemarqueCIN, 2);
            }
            return Validation_function(CIN_patient, RemarqueCIN, 2);
        }

        //-----------------------  END  METHODES  :  ------------------------------------ ----


    }
}
