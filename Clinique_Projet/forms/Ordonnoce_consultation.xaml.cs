using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour Ordonnoce_consultation.xaml
    /// </summary>
    public partial class Ordonnoce_consultation : Window
    {
        public int ConsultID;
       
        //list va contenient tous les ordonnance de consultation 
        ObservableCollection<GestionOrdonnance_Class> List_Ordonnance = new ObservableCollection<GestionOrdonnance_Class>();

        //cette objet pour Selectionner  ordonnace
        public GestionOrdonnance_Class? ObjOrdonnance = null ; 

        public Utilisateur_Class user;
        // DMOrdonnance : recureper les modifcations sur la table bilans
        public SqlTableDependency<DMOrdonnance> OrdonnanceSqlTableDependency;
        // DMBilans : recureper les modifcations sur la table consulttion
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;
        public Ordonnoce_consultation(int idconsult,Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                ConsultID = idconsult;
                loadCategorie_Medicament();
                LoadData_Ordonnoces();
                Load_Combobox_Module();
                DeleteOrdanncement_btn.IsEnabled = false;
                EditOrdanncement_btn.IsEnabled=false;
                Quantite_medicament.Text = "1";
                this.user = user;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- SATRT EVENTS :  -------------------------------
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    Task.Run(() => Stop_TableConsultation_Dependency()),
                    Task.Run(() => Stop_TableOrdonnance_Dependency())
                );
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
                await Task.WhenAll(
                    Task.Run(() => Start_TableConsultation_Dependency()),
                    Task.Run(() => Start_TableOrdonnance_Dependency())
                );
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler btn
        private void btn_annuler_ordonnonce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur annuler  cette operation ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //combobox  de module
        private void combobox_Module_Ordonnonce_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_Module_Ordonnonce.SelectedItem != null)
                {
                    if (Convert.ToInt32(combobox_Module_Ordonnonce.SelectedValue) != -1)
                    {
                        btn_Valider_Module.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btn_Valider_Module.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // appliquer un module
        private void btn_Valider_Module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous Appliquer ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (Appliquer_module_Medicament())
                    {
                        new succes().ShowDialog();
                        Annuler_OperartionOrdonnace();
                        combobox_Module_Ordonnonce.SelectedIndex = 0;
                        btn_Valider_Module.Visibility = Visibility.Collapsed;
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ajouter un nouveau  module
        private void btn_add_Module_Ordonnonce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur d'effectue cette operation  ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    // add le module
                    string nom_module_Ordonnonce = "Nom Module";

                    // get le nom de module from the window 
                    Nom_Module_Window f = new Nom_Module_Window();
                    f.ShowDialog();
                    nom_module_Ordonnonce = f.name_module;

                    if (nom_module_Ordonnonce != "")
                    {
                        // add the module 
                        Module_Ordonnonce_Class new_module_ordonnonce = new Module_Ordonnonce_Class(0, nom_module_Ordonnonce);
                        if (new_module_ordonnonce.Add_Module_Ordonnonce())
                        {
                            // get the new add
                            int id_Module = Module_Ordonnonce_Class.GetLast_Id();
                            if (id_Module != 0)
                            {
                                // add les medicament dans le module
                                foreach (var item in List_Ordonnance) Module_Ordonnonce_Class.Add_Medicament_on_module(id_Module, item.Medicament.IdMedcament);
                                new succes().ShowDialog();
                            }
                            else MessageBox.Show("Opération d'entrée innatendu !!");
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // add new ordonnace
        private void AddOrdanncement_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_Medicament;
                if ((comboboxMedicament.SelectedItem != null) && (Validation_function(Quantite_medicament, RemarqueQunatite, 8)))
                {
                    id_Medicament = Convert.ToInt32(comboboxMedicament.SelectedValue.ToString());

                    if (!Medicament_deja_exist(id_Medicament))
                    {
                        // confirmation
                        MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Médicament", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            // Ajouter dans base de donnee
                            Ordonnace_Class ordonnace = new Ordonnace_Class(ConsultID, id_Medicament, DateTime.Now
                                    , posologie_medicament.Text, Notes_medicament.Text, Convert.ToInt32(Quantite_medicament.Text));
                            if (ordonnace.AddOrdonnance())
                            {
                                MessageBox.Show("Ordonnonce est bien Ajouter ");

                                // Ajouter un notification
                                Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Ajouter  un ordonnonce de consutation");
                                notification_.Add_Notification();

                                // sql Dependecy  actualiser le datagrid view
                                Annuler_OperartionOrdonnace();
                            }
                        }
                    }
                    else MessageBox.Show("deja existe");
                }
                else
                {
                    MessageBox.Show(" les données sont invlides ! ");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //show info ordonnace
        private void showInfo_Ordanncement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Ordanncements.SelectedItem != null)
                {
                    ObjOrdonnance = (GestionOrdonnance_Class)datagrid_Ordanncements.SelectedItem;

                    //remplir les champs
                    posologie_medicament.Text = ObjOrdonnance.ordonnance.Posologie_Ordonnace;
                    Notes_medicament.Text = ObjOrdonnance.ordonnance.Note_Plus;
                    comboboxType_Medicament.Text = ObjOrdonnance.CatMedicament.Nom_CatMedicament;
                    comboboxMedicament.Text = ObjOrdonnance.Medicament.NomMedcament;
                    Quantite_medicament.Text = ObjOrdonnance.ordonnance.Quantite.ToString();

                    //disable combobox
                    comboboxMedicament.IsEnabled = false;
                    comboboxType_Medicament.IsEnabled = false;

                    //enable edit and delete
                    DeleteOrdanncement_btn.IsEnabled = true;
                    EditOrdanncement_btn.IsEnabled = true;
                    AddOrdanncement_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // edit new ordonnace
        private void EditOrdanncement_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((comboboxMedicament.SelectedItem != null) && (Validation_function(Quantite_medicament, RemarqueQunatite, 8)))
                {
                    // confirmation
                    MessageBoxResult res = MessageBox.Show("Voullez vous Modifier  cet Ordonnonce", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (ObjOrdonnance != null)
                        {
                            //modifier dans la base de donnee
                            Ordonnace_Class ordonnace = new Ordonnace_Class(ConsultID, ObjOrdonnance.Medicament.IdMedcament, posologie_medicament.Text
                                        , Notes_medicament.Text, Convert.ToInt32(Quantite_medicament.Text));
                            if (ordonnace.UpdateOrdonnance())
                            {
                                MessageBox.Show("ordonnonce est bien modifier ");
                                //sql dependecy va modifier dans la list des ordonnonces 

                                //Ajouter une notification
                                Notification_class notification_ =
                                    new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Modifié un ordonnonce de consutation");
                                notification_.Add_Notification();
                            }
                        }
                        else
                        {
                            MessageBox.Show(" l'Opértion est annulée ");
                        }
                        Annuler_OperartionOrdonnace();
                    }
                }
                else
                {
                    MessageBox.Show(" les données sont invlides ! ");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // delete new ordonnace
        private void DeleteOrdanncement_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de supprimer cet Ordonnonce ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjOrdonnance != null)
                    {
                        //delete dans la base de donnee
                        if (Ordonnace_Class.DeleteOrdonnace(ConsultID, ObjOrdonnance.Medicament.IdMedcament))
                        {
                            MessageBox.Show("Ordonnonce est bien supprimer ");
                            //sql dependecy va actualiser le datagrid  

                            //Ajouter une notifications
                            Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Supprimer un ordonnonce de consutation");
                            notification_.Add_Notification();
                        }
                    }
                    else
                    {
                        MessageBox.Show(" l'Opértion est annulée ");
                    }
                    Annuler_OperartionOrdonnace();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // annuler btn
        private void AnnulerOrdanncement_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Annuler_OperartionOrdonnace();
            }
            catch
            (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //combobox type medicament=>selection de le medicament par type
        private void comboboxType_Medicament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectMedicament_ByCat();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // valide la quantite de medicaments
        private void Quantite_medicament_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                Validation_function(Quantite_medicament, RemarqueQunatite, 8);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //parametres  des  medicaments
        private void add_medicament_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Ajouter_medicament(user).ShowDialog();
                loadCategorie_Medicament();
                SelectMedicament_ByCat();
                Load_Combobox_Module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        // -------------------------- END EVENTS   -------------------------------




        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
 
        // ----------- start sql dependecy : Consultation : le cas de suppression  -------------
        private void Start_TableConsultation_Dependency()
        {
            try
            {
                if (ConsultationSqlTableDependency == null)
                {
                    ConsultationSqlTableDependency = new SqlTableDependency<DMConsultation>(ConnectDb.GetConnectionstring(), "Consultation");
                    ConsultationSqlTableDependency.OnChanged += Changed_Consultation_Dependency;
                    ConsultationSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Consultation_Dependency(object sender, RecordChangedEventArgs<DMConsultation> e)
        {
            try
            {
                DMConsultation EntityConsultation = e.Entity;
                if ((e.ChangeType == ChangeType.Delete) && (EntityConsultation.id_Consult == ConsultID))
                {
                    Dispatcher.Invoke(() =>
                    {
                        new Notification_window(" Cette  Consultation  a ete  supprimer  ")
                                                       .ShowDialog();
                        Close();
                    });
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_TableConsultation_Dependency()
        {
            try
            {
                ConsultationSqlTableDependency?.Stop();
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy : Consultation -------------

        // ----------- start sql dependecy : Ordonnance  -------------
        private void Start_TableOrdonnance_Dependency()
        {
            try
            {
                if (OrdonnanceSqlTableDependency == null)
                {
                    OrdonnanceSqlTableDependency = new SqlTableDependency<DMOrdonnance>(ConnectDb.GetConnectionstring(), "Ordonnance");
                    OrdonnanceSqlTableDependency.OnChanged += Changed_Ordonnance_Dependency;
                    OrdonnanceSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Changed_Ordonnance_Dependency(object sender, RecordChangedEventArgs<DMOrdonnance> e)
        {
            try
            {
                DMOrdonnance EntityOrdonnance = e.Entity;
                if (EntityOrdonnance != null)
                {
                    if (EntityOrdonnance.Consult_id == ConsultID)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                    Medicament_Class medicament = Medicament_Class.Select_Medicament(EntityOrdonnance.Medicament_id);
                                    List_Ordonnance.Add(new GestionOrdonnance_Class
                                    {
                                        Medicament = new Medicament_Class { IdMedcament = medicament.IdMedcament , NomMedcament = medicament.NomMedcament },
                                        CatMedicament = new Categorie_Medicament {  Nom_CatMedicament= medicament.CatMedcament },
                                        ordonnance = new Ordonnace_Class { Posologie_Ordonnace = EntityOrdonnance.Posologie , DateOrdonnace = EntityOrdonnance.Date_Ordonnace 
                                                    , Note_Plus = EntityOrdonnance.Note_Supplimentaire , Quantite = EntityOrdonnance.Quantite , Consult_Ordonnace = ConsultID },
                                    });;
                                    datagrid_Ordanncements.ItemsSource = null;
                                    datagrid_Ordanncements.ItemsSource = List_Ordonnance;
                                    show_image_aucun_ordonnonce();

                                    if (ObjOrdonnance != null)
                                    {
                                        SelectionItemOrdonnonce(ObjOrdonnance.Medicament.IdMedcament);
                                    }
                                });
                                break;

                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Ordonnance)
                                    {
                                        if (item.Medicament.IdMedcament == EntityOrdonnance.Medicament_id)
                                        {
                                            item.ordonnance.Posologie_Ordonnace = EntityOrdonnance.Posologie;
                                            item.ordonnance.Note_Plus = EntityOrdonnance.Note_Supplimentaire;
                                            item.ordonnance.Quantite = EntityOrdonnance.Quantite;
                                            // si le ordonnonce qui modifier etait selectionner
                                            if ((ObjOrdonnance != null) && (ObjOrdonnance.Medicament.IdMedcament == EntityOrdonnance.Medicament_id))
                                                posologie_medicament.Text = EntityOrdonnance.Posologie;
                                                Notes_medicament.Text = EntityOrdonnance.Note_Supplimentaire;
                                                Quantite_medicament.Text = EntityOrdonnance.Quantite.ToString();
                                            return;
                                        }
                                    }
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Ordonnance)
                                    {
                                        if (item.Medicament.IdMedcament == EntityOrdonnance.Medicament_id)
                                        {
                                            List_Ordonnance.Remove(item);
                                            break;
                                        }
                                    }

                                    datagrid_Ordanncements.ItemsSource = null;
                                    datagrid_Ordanncements.ItemsSource = List_Ordonnance;
                                    show_image_aucun_ordonnonce();

                                    if (ObjOrdonnance != null)
                                    {
                                        // si on supprimer analyse qui ete selectionner par utilisateur
                                        if (ObjOrdonnance.Medicament.IdMedcament == EntityOrdonnance.Medicament_id)
                                        {
                                            Annuler_OperartionOrdonnace();
                                        }
                                        else
                                        {
                                            SelectionItemOrdonnonce(ObjOrdonnance.Medicament.IdMedcament);
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
        private void Stop_TableOrdonnance_Dependency()
        {
            try
            {
                OrdonnanceSqlTableDependency?.Stop();
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy : Ordonnance -------------

        private bool Appliquer_module_Medicament()
        {
            try
            {
                //selection le module
                Module_Ordonnonce_Class module = (Module_Ordonnonce_Class)combobox_Module_Ordonnonce.SelectedItem;

                // get les analyse
                ObservableCollection<int> list_ID_Ordonnonce = Module_Ordonnonce_Class.Display_Ordonnonce_Module(module.Id_Module_Ordonnonce);
               
                //remplir la list 
                if (List_Ordonnance.Count > 0)
                {
                    MessageBoxResult res = MessageBox.Show("Voullez vous Sauvgarder les Ordonnonces recents", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.No)
                    {
                        foreach (var item in List_Ordonnance) Ordonnace_Class.DeleteOrdonnace(ConsultID, item.Medicament.IdMedcament);
                         List_Ordonnance.Clear();
                    }
                }
                foreach (var ID_Medecament in list_ID_Ordonnonce)
                {
                    Medicament_Class medicament = Medicament_Class.Select_Medicament(ID_Medecament);
                    if (!Medicament_deja_exist(ID_Medecament))
                    {
                        // add dans base de donnee
                        Ordonnace_Class ordonnace = new Ordonnace_Class(ConsultID, ID_Medecament, DateTime.Now, "", "", 1);
                        ordonnace.AddOrdonnance();
                        // sql dpendecy va ajouter dans la list  
                    }
                    else MessageBox.Show("le medicament " + medicament.NomMedcament + " est deja exist");
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void Load_Combobox_Module()
        {
            try
            {
                combobox_Module_Ordonnonce.ItemsSource = Module_Ordonnonce_Class.Display_Module_Ordonnonce();
                combobox_Module_Ordonnonce.SelectedValuePath = "Id_Module_Ordonnonce";
                combobox_Module_Ordonnonce.DisplayMemberPath = "Nom_Module_Ordonnonce";
                combobox_Module_Ordonnonce.SelectedIndex = 0;
            }
            catch (Exception )
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private bool Medicament_deja_exist(int id)
        {
            foreach (var item in List_Ordonnance)
            {
                if (item.Medicament.IdMedcament == id) return true;
            }
            return false;
        }

        //load data of Ordonnoces
        private void LoadData_Ordonnoces()
        {
            try
            {
                //load ordonnace
                List_Ordonnance = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(ConsultID);
                datagrid_Ordanncements.ItemsSource = List_Ordonnance;
                show_image_aucun_ordonnonce();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void show_image_aucun_ordonnonce()
        {
            try
            {
                if (List_Ordonnance.Count <= 0)
                {
                    border_image_Aucun_ordonnoces.Visibility = Visibility.Visible;
                }
                else border_image_Aucun_ordonnoces.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load_type_medicament
        private void loadCategorie_Medicament()
        {
            try
            {
                comboboxType_Medicament.ItemsSource = Categorie_Medicament.Display_CatMedicaments();
                comboboxType_Medicament.SelectedValuePath = "ID_CatMedicament";
                comboboxType_Medicament.DisplayMemberPath = "Nom_CatMedicament";
                comboboxType_Medicament.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //filtre medicament by categorie de medicament
        private void SelectMedicament_ByCat()
        {
            try
            {
                if (comboboxType_Medicament.SelectedValue != null)
                {
                    comboboxMedicament.ItemsSource = null;
                    int id_Cat = (int)comboboxType_Medicament.SelectedValue;
                    comboboxMedicament.ItemsSource = Medicament_Class.Display_Medicament_ByCat(id_Cat);
                    comboboxMedicament.SelectedValuePath = "IdMedcament";
                    comboboxMedicament.DisplayMemberPath = "NomMedcament";
                    comboboxMedicament.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //initiliser les champs:
        private void Annuler_OperartionOrdonnace()
        {
            try
            {
                datagrid_Ordanncements.SelectedIndex = -1;
                ObjOrdonnance = null;

                // Initialiser combobox
                comboboxType_Medicament.IsEnabled = true;
                comboboxMedicament.IsEnabled = true;
                comboboxType_Medicament.SelectedIndex = 0;
                comboboxMedicament.SelectedIndex = 0;

                // Initialiser les champs
                posologie_medicament.Text = "";
                Quantite_medicament.Text = "1";
                Notes_medicament.Text = "";

                //Initialiser buttons : 
                EditOrdanncement_btn.IsEnabled = false;
                DeleteOrdanncement_btn.IsEnabled = false;
                AddOrdanncement_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //selectionner par index 
        private void SelectionItemOrdonnonce(int IdMedicament)
        {
            foreach (GestionOrdonnance_Class item in datagrid_Ordanncements.Items)
            {
                if (item.Medicament.IdMedcament == IdMedicament)
                {
                    datagrid_Ordanncements.SelectedIndex = datagrid_Ordanncements.Items.IndexOf(item);
                    break;
                }
            }
        }

        // --------------validation -------------
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

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
