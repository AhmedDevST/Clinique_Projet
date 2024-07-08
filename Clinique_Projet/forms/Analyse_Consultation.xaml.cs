using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using System.Threading.Tasks;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Analyse_Consultation.xaml
    /// </summary>
    public partial class Analyse_Consultation : Window
    {
        public static int ConsultID;
       
        //list va contenient tous les results des analyse de consultation
        ObservableCollection<GestionBilan_class> List_Bilans = new ObservableCollection<GestionBilan_class>();
        
        //cette objet pour selection d une bilan
        public GestionBilan_class? ObjBilan = null; 

        // utilisateur qui fait les actions
        public Utilisateur_Class user;

        // DMBilans : recureper les modifcations sur la table bilans
        public SqlTableDependency<DMBilans> BilansSqlTableDependency;

        // DMBilans : recureper les modifcations sur la table consulttion
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;
        public Analyse_Consultation(int idconsult, Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                ConsultID = idconsult;
                LoadData_Bilans();
                Load_TypeAnalyse();
                Load_Combobox_Module();
                EditBilan_btn.IsEnabled = false;
                DeleteBilan_btn.IsEnabled = false;
                this.user = user;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

       
       
    // -----------------------  SATRT  EVENTS ------------------------------------
        private void combobox_Module_Analyse_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_Module_Analyse.SelectedItem != null)
                {
                    if (Convert.ToInt32(combobox_Module_Analyse.SelectedValue) != -1)
                    {
                        btn_Valider_Module.Visibility = Visibility.Visible; //selection un module
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

        //appliquer un module d anlyse
        private void btn_Valider_Module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous Appliquer ce module", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (Appliquer_module_Analyse())
                    {
                        new succes().ShowDialog();
                        Annuler_OperartionBilans();
                        combobox_Module_Analyse.SelectedIndex = 0;
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

        // button : enregistre les analyses comme un  module
        private void btn_Ajouter_Module_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur d'effectue cette operation  ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    string nom_module_analyse = "Nom Module";
                    // get le nom de module from the window 
                    Nom_Module_Window f = new Nom_Module_Window();
                    f.ShowDialog();
                    nom_module_analyse = f.name_module;

                    // add the module 
                    Module_Analyse_Class new_module_analyse = new Module_Analyse_Class(0, nom_module_analyse);
                    if (new_module_analyse.Add_Module_Analyse())
                    {
                        // get the new add
                        int id_Module = Module_Analyse_Class.GetLast_Id();
                        if (id_Module != 0)
                        {
                            // add les analyse dans le module
                            foreach (var item in List_Bilans) Module_Analyse_Class.Add_Analyse_on_module(id_Module, item.Analyse.IdAnalyse);
                            new succes().ShowDialog();
                            this.Close();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //close window
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    Task.Run(() => Stop_TableConsultation_Dependency()),
                    Task.Run(() => Stop_TableBilans_Dependency())
                 );    
            }
            catch (Exception)
            {
            }
        }
        //open window 
        private  async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                 Task.Run(() => Start_TableConsultation_Dependency()),
                 Task.Run(() => Start_TableBilans_Dependency())
                 );
            }
            catch (Exception)
            {
            }
        }

        //button close  window
        private void btn_annuler_bilans_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de quitter  ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        //ajouter  un   bilan
        private void AddBilan_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboboxBilan.SelectedItem != null)
                {
                    // stocker id de analyse depuis le combobox selectionner
                    int id_Analyse = Convert.ToInt32(comboboxBilan.SelectedValue.ToString());

                    // si analyse n est pas exist dans la list 
                    if (!Analyse_deja_exist(id_Analyse))
                    {
                        // confirmation
                        MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            // Ajouter dans base de donnee
                            BilansClass bilans = new BilansClass(ConsultID, id_Analyse, result_bilans.Text, DateTime.Now);
                            if (bilans.AddBilan())
                            {
                                // Ajouter un notification
                                Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Ajouter  des Analyses de consutation");
                                notification_.Add_Notification();
                                // sql Dependecy  actualiser le datagrid view
                                Annuler_OperartionBilans();
                            }
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

        //modifier bilan
        private void EditBilan_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // confirmation
                MessageBoxResult res = MessageBox.Show("Voullez vous Modifier Les Resultats de cet analyse", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjBilan != null)
                    {
                        //modifier dans la base de donnee
                        BilansClass bilans = new BilansClass(ConsultID, ObjBilan.Analyse.IdAnalyse, result_bilans.Text);
                        if (bilans.UpdateBilans())
                        {
                            //sql dependecy va modifier dans la list des bilan 
                            //Ajouter une notification
                            Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Modifié des Analyses de consutation");
                            notification_.Add_Notification();
                        }
                    }
                    else
                    {
                        MessageBox.Show(" l'Opértion est annulée ");
                    }
                    Annuler_OperartionBilans();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // show bilan info
        private void show_bilan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_bilans.SelectedItem != null)
                {
                    ObjBilan = (GestionBilan_class)datagrid_bilans.SelectedItem;

                    //remplir les champs
                    result_bilans.Text = ObjBilan.Bilans.Result_Analyse;
                    comboboxType_Bilan.Text = ObjBilan.TypeAnalyse.Nom_TypeBilan;
                    comboboxBilan.Text = ObjBilan.Analyse.NomAnalyse;

                    //disable combobox
                    comboboxBilan.IsEnabled = false;
                    comboboxType_Bilan.IsEnabled = false;

                    //enable edit and delete
                    Border_add_bialns.Visibility = Visibility.Visible;
                    DeleteBilan_btn.IsEnabled = true;
                    EditBilan_btn.IsEnabled = true;
                    AddBilan_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //Annuler btn
        private void AnnulerBilan_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Annuler_OperartionBilans();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //delete bilans
        private void DeleteBilan_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de supprimer cet analyse ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjBilan != null)
                    {
                        //delete dans la base de donnee
                        if (BilansClass.DeleteBilans(ConsultID, ObjBilan.Analyse.IdAnalyse))
                        {
                            //sql dependecy va actualiser le datagrid  
                            //Ajouter une notifications
                            Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Supprimer des Analyses de consutation");
                            notification_.Add_Notification();
                        }
                    }
                    else
                    {
                        MessageBox.Show(" l'Opértion est annulée ");
                    }
                    Annuler_OperartionBilans();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // event sur combobox type analyses : select les analyses par  leurs  type 
        private void comboboxType_Bilan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectAnalyse_ByType();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //ouvrir fenetre de Parametres  des ananlyses
        private void add_analyse_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new Ajouter_Analyse(user).ShowDialog();
                Load_TypeAnalyse();
                SelectAnalyse_ByType();
                Load_Combobox_Module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
    // -----------------------  END   EVENTS -----------------------------------------------

    
        
    //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        //appliquer un modele de analyse : ajouter les analyse de modele au list de bilans
        private bool Appliquer_module_Analyse()
        {
            try
            {
                //selection le module
                Module_Analyse_Class module = (Module_Analyse_Class)combobox_Module_Analyse.SelectedItem;

                // get les analyse de module
                ObservableCollection<int> list_ID_analyse = Module_Analyse_Class.Display_Analyse_Module(module.Id_Module_Analyse);

                //sauvgarder ou supprimer  les ancienne analyse de  list 
                if (List_Bilans.Count > 0)
                {
                    MessageBoxResult res = MessageBox.Show("Voullez vous Sauvgarder les analyses recents", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.No)
                    {
                        //supprimer les analyses dans la base de donnees
                        foreach (var item in List_Bilans) BilansClass.DeleteBilans(ConsultID, item.Analyse.IdAnalyse);
                        //vider la list
                        List_Bilans.Clear();
                    }
                }

                // ajouter les analyse de module dans la list 
                foreach (var ID_ANALYSE in list_ID_analyse)
                {
                    AnalyseClass analyse = AnalyseClass.Select_Analyse(ID_ANALYSE);
                    if (!Analyse_deja_exist(ID_ANALYSE))
                    {
                        // add dans base de donnee
                        BilansClass bilans = new BilansClass(ConsultID, ID_ANALYSE, "", DateTime.Now);
                        bilans.AddBilan();
                        // sql dpendecy va ajouter dans la list         
                    }
                    else MessageBox.Show("l'Analyse " + analyse.NomAnalyse + " est deja exist");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // tester si un analyse n exist pas dej exist au list de bilans
        // true :  exist
        // false : not exist  
        private bool Analyse_deja_exist(int id)
        {
            foreach (var item in List_Bilans)
            {
                if (item.Analyse.IdAnalyse == id) return true;
            }
            return false;
        }

        // laod combobox modules des analyses
        private void Load_Combobox_Module()
        {
            try
            {
                combobox_Module_Analyse.ItemsSource = Module_Analyse_Class.Display_Module_Analyse();
                combobox_Module_Analyse.SelectedValuePath = "Id_Module_Analyse";
                combobox_Module_Analyse.DisplayMemberPath = "Nom_Module_Analyse";
                combobox_Module_Analyse.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load les bilans de consultation dans la list
        private void LoadData_Bilans()
        {
            try
            {
                List_Bilans = GestionBilan_class.DisplayResultsBilans_Datagrid(ConsultID);
                datagrid_bilans.ItemsSource = List_Bilans;
                show_image_aucun_analyse();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // afficher un  image si on a pas des analyse au list
        private void show_image_aucun_analyse()
        {
            if (List_Bilans.Count <= 0)
            {
                border_image_Aucun_analyse.Visibility = Visibility.Visible;
            }
            else border_image_Aucun_analyse.Visibility = Visibility.Collapsed;
        }

        //load combobox type Analyse
        private void Load_TypeAnalyse()
        {
            try
            {
                comboboxType_Bilan.ItemsSource = TypeAnalyse.Display_TypesAnlyse();
                comboboxType_Bilan.SelectedValuePath = "ID_TypeBilan";
                comboboxType_Bilan.DisplayMemberPath = "Nom_TypeBilan";
                comboboxType_Bilan.SelectedIndex = 0;
            }
            catch (Exception )
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //filtre combobox Analyse by type analyse  selectionner on combobox type analyse
        private void SelectAnalyse_ByType()
        {
            try
            {
                if (comboboxType_Bilan.SelectedValue != null)
                {
                    comboboxBilan.ItemsSource = null;
                    int id_type = (int)comboboxType_Bilan.SelectedValue;
                    comboboxBilan.ItemsSource = AnalyseClass.Display_Analyse_ByType(id_type);
                    comboboxBilan.SelectedValuePath = "IdAnalyse";
                    comboboxBilan.DisplayMemberPath = "NomAnalyse";
                    comboboxBilan.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //selectionner un element au data grid par id analyse 
        private void SelectionItemBilan(int IdAnalyse)
        {
            foreach (GestionBilan_class item in datagrid_bilans.Items)
            {
                if (item.Analyse.IdAnalyse == IdAnalyse)
                {
                    datagrid_bilans.SelectedIndex = datagrid_bilans.Items.IndexOf(item);
                    break;
                }
            }
        }

        // iniasilaiser les champs : combobox, buttons , champs text , ObjBilan ,  datagrid_bilans
        private void Annuler_OperartionBilans()
        {
            try
            {
                //selection reien a la datagrid
                datagrid_bilans.SelectedIndex = -1;
                result_bilans.Text = "";
                ObjBilan = null;

                //Initialiser combobox
                comboboxBilan.IsEnabled = true;
                comboboxType_Bilan.IsEnabled = true;
                comboboxType_Bilan.SelectedIndex = 0;
                comboboxBilan.SelectedIndex = 0;

                //Initialiser buttons : 
                EditBilan_btn.IsEnabled = false;
                DeleteBilan_btn.IsEnabled = false;
                AddBilan_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ----------- start sql dependecy : Consultation : le cas de suppression  -------------
        private  void   Start_TableConsultation_Dependency()
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

        // ----------- start sql dependecy : Bilans  -------------
        private void Start_TableBilans_Dependency()
        {
            try
            {
                if (BilansSqlTableDependency == null)
                {
                    BilansSqlTableDependency = new SqlTableDependency<DMBilans>(ConnectDb.GetConnectionstring(), "Bilans");
                    BilansSqlTableDependency.OnChanged += Changed_Bilans_Dependency;
                    BilansSqlTableDependency.Start();     
                }
               
            }
            catch (Exception)
            {
               
                
            }
        }
        private void Changed_Bilans_Dependency(object sender, RecordChangedEventArgs<DMBilans> e)
        {
            try
            {
                DMBilans EntityBilans = e.Entity;
                if (EntityBilans != null)
                {
                    if (EntityBilans.Consult_id == ConsultID)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                    AnalyseClass analyse = AnalyseClass.Select_Analyse(EntityBilans.Analyse_id);
                                    List_Bilans.Add(new GestionBilan_class
                                    {
                                        Analyse = new AnalyseClass { IdAnalyse = EntityBilans.Analyse_id, NomAnalyse = analyse.NomAnalyse },
                                        TypeAnalyse = new TypeAnalyse { Nom_TypeBilan = analyse.TypeAnalyse },
                                        Bilans = new BilansClass { Result_Analyse = EntityBilans.Result_Analyse, DateBilan = EntityBilans.Date_Bilan, ConsultID = ConsultID },

                                    });

                                    datagrid_bilans.ItemsSource = null;
                                    datagrid_bilans.ItemsSource = List_Bilans;
                                    show_image_aucun_analyse();

                                    if (ObjBilan != null)
                                    {
                                        SelectionItemBilan(ObjBilan.Analyse.IdAnalyse);
                                    }
                                     MessageBox.Show(" un analyse est  Ajoutée ");
                                });
                                break;

                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Bilans)
                                    {
                                        if (item.Analyse.IdAnalyse == EntityBilans.Analyse_id)
                                        {
                                            item.Bilans.Result_Analyse = EntityBilans.Result_Analyse;
                                            // si le bilans qui modifier etait selectionner
                                            if ((ObjBilan != null) && (ObjBilan.Analyse.IdAnalyse == EntityBilans.Analyse_id))
                                                result_bilans.Text = EntityBilans.Result_Analyse;
                                            MessageBox.Show("un analyse est  modifier ");
                                            return;
                                        }
                                    }
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var item in List_Bilans)
                                    {
                                        if (item.Analyse.IdAnalyse == EntityBilans.Analyse_id)
                                        {
                                            List_Bilans.Remove(item);
                                            break;
                                        }
                                    }

                                    datagrid_bilans.ItemsSource = null;
                                    datagrid_bilans.ItemsSource = List_Bilans;
                                    show_image_aucun_analyse();

                                    if (ObjBilan != null)
                                    {
                                        // si on supprimer analyse qui ete selectionner par utilisateur
                                        if (ObjBilan.Analyse.IdAnalyse == EntityBilans.Analyse_id)
                                        {
                                            Annuler_OperartionBilans();
                                        }
                                        else
                                        {
                                            SelectionItemBilan(ObjBilan.Analyse.IdAnalyse);
                                        }
                                    }
                                    MessageBox.Show("un analyse est  supprimée ");
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
        private void Stop_TableBilans_Dependency()
        {
            try
            {
                BilansSqlTableDependency?.Stop();
            }
            catch (Exception)
            {
            }
        }

        // ----------- End sql dependecy : bilans -------------

       
        //-----------------------  END  METHODES  --------------------------------------
    }
}
