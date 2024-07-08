using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.connectionDb;
using System.Threading.Tasks;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour BIlans_Controlle_secritaire.xaml
    /// </summary>
    public partial class BIlans_Controlle_secritaire : UserControl
    {
        public  static int ConsultID;

        //list va contenient tous les results des analyse de consultation
        ObservableCollection<GestionBilan_class> List_Bilans = new ObservableCollection<GestionBilan_class>();
        
        //cette objet pour selection d une bilan
        public GestionBilan_class? ObjBilan = null; 
        
        // utilisateur qui fait les actions
        public Utilisateur_Class user;
        
        // DMBilans : recureper les modifcations sur la table conConsultation
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;
        
        // DMBilans : recureper les modifcations sur la table bilans
        public SqlTableDependency<DMBilans> BilansSqlTableDependency;
        public BIlans_Controlle_secritaire(int idc,Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                ConsultID = idc;
                EditBilan_btn.IsEnabled = false;
                DeleteBilan_btn.IsEnabled = false;
                Load_Bilans();
                Load_TypeAnalyse();
                SelectAnalyse_ByType();
                ConsultationClass consult = ConsultationClass.SelectConsultation(ConsultID);
                NamePatient.Text = PatientClass.SelectNomPatient(consult.ID_patient);
                DateConsultation.Text = consult.DateConsult.ToString("dddd dd MMMM yyyy ", new CultureInfo("fr-FR"));
                this.user = user;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }       
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        private async void UserControl_Unloaded(object sender, RoutedEventArgs e)
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

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
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

        //Fermer btn
        private void btn_annuler_bilans_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de quitter  ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                Acceuil acceuil = Window.GetWindow(this) as Acceuil;
                if (acceuil != null)
                {
                    acceuil.ChangerControleur(new consultation_sec(user));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //add new bilan
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
                                MessageBox.Show("analyse est bien Ajouter ");

                                // Ajouter un notification
                                Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Ajouter  un  Analyse au consutation");
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

        //edit bilan
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
                            MessageBox.Show("analyse est bien modifier ");
                            //sql dependecy va modifier dans la list des bilan 

                            //Ajouter une notification
                            Notification_class notification_ =
                                new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Modifié un  Analyses de consutation");
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

        //Annuler btn bilans
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
                            MessageBox.Show("analyse est bien supprimer ");
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

        // event combobox selected the bilan of a type 
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
                if ((e.ChangeType == ChangeType.Delete) && (EntityConsultation.id_Consult == ConsultID) )
                {
                    Dispatcher.Invoke(() =>
                    {
                        new Notification_window(" Cette  Consultation  a ete  supprimer  ")
                                                .ShowDialog();
                        consultation_sec consult = new consultation_sec(user);
                        Acceuil acceuil = Window.GetWindow(this) as Acceuil;
                        if (acceuil != null) acceuil.ChangerControleur(consult);
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
            catch (Exception )
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
                if (BilansSqlTableDependency != null)
                {
                    BilansSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }
       
        // ----------- End sql dependecy : Bilans  -------------  
        private void Load_Bilans()
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

        //load type Analyse
        private void Load_TypeAnalyse()
        {
            comboboxType_Bilan.ItemsSource = TypeAnalyse.Display_TypesAnlyse();
            comboboxType_Bilan.SelectedValuePath = "ID_TypeBilan";
            comboboxType_Bilan.DisplayMemberPath = "Nom_TypeBilan";
            comboboxType_Bilan.SelectedIndex = 0;
        }

        //filtre Analyse by type analyse
        private void SelectAnalyse_ByType()
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
        
        private void show_image_aucun_analyse()
        {
            if (List_Bilans.Count <= 0)
            {
                border_image_Aucun_analyse.Visibility = Visibility.Visible;
            }
            else border_image_Aucun_analyse.Visibility = Visibility.Collapsed;
        }
       
        // iniasilaiser les champs
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

                //Initialiser buttons : modifier && supprimer
                EditBilan_btn.IsEnabled = false;
                DeleteBilan_btn.IsEnabled = false;
                AddBilan_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        private bool Analyse_deja_exist(int id)
        {
            foreach (var item in List_Bilans)
            {
                if (item.Analyse.IdAnalyse == id) return true;
            }
            return false;
        }
        
        //selectuionner par index 
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
        //-----------------------  END  METHODES    ------------------------------------ ----

    }
}
