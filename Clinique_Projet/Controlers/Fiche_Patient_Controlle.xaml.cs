using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using System.Threading.Tasks;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Fiche_Patient_Controlle.xaml
    /// </summary>
    public partial class Fiche_Patient_Controlle : UserControl
    {
        public static int idPatient { get; set; }
        public int IdConsultation { get; set; }
        ObservableCollection<ConsultationClass> Consult_List;
        ObservableCollection<Gestion_Antecedent> Antecdent_list;
        public Utilisateur_Class user;

        public SqlTableDependency<DMBilans> BilansSqlTableDependency;
        public SqlTableDependency<DMOrdonnance> OrdonnanceSqlTableDependency;
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;
        public SqlTableDependency<DMPatient> PatientSqlTableDependency;
        public SqlTableDependency<DMAntecedents> AntecedentsSqlTableDependency;
        public Fiche_Patient_Controlle(int id,Utilisateur_Class user)
        {
            try 
            {
                InitializeComponent();
                idPatient = id;
                this.user = user;
                Load_Fiche_Patient();
                Droits_user();
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
                    Task.Run(() => Stop_TablePatient_Dependency()),
                    Task.Run(() => Stop_TableAntecedents_Dependency()),
                    Task.Run(() => Stop_TableConsultation_Dependency()),
                    Task.Run(() => Stop_TableBilans_Dependency()),
                    Task.Run(() => Stop_TableOrdonnance_Dependency())
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
                     Task.Run(() => Start_TablePatient_Dependency()),
                     Task.Run(() => Start_TableAntecedents_Dependency()),
                     Task.Run(() => Start_TableConsultation_Dependency()),
                     Task.Run(() => Start_TableBilans_Dependency()),
                     Task.Run(() => Start_TableOrdonnance_Dependency())
                 );
            }
            catch (Exception)
            {
            }
        }

        //delete patient
        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez vous supprimer ce patient?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (PatientClass.DeletePatient(idPatient))
                    {
                        new succes().ShowDialog();
                        PatientControlle_Medecin p = new PatientControlle_Medecin(user);
                        Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                        if (acceuil != null)
                        {
                            acceuil.ChangerControleur(p);
                        }
                    }
                    else MessageBox.Show("Opération d'entrée innatendu !!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir fenetre de  details bilans
        private void DetailsBilan_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Bilans.SelectedItem != null)
                {
                    GestionBilan_class res = (GestionBilan_class)datagrid_Bilans.SelectedItem;
                    DeatilsBilan f = new DeatilsBilan(res);
                    f.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button : afficher les  info  d une consultation en data grid
        private void Show_infoConsult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Show_Info_Select_Consult();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button : afficher les  info  d une Antecdent en data grid
        private void ShowInfo_Anteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_TypeANtecedent.SelectedItem != null)
                {
                    TypeAntecedent type_anteced = (TypeAntecedent)datagrid_TypeANtecedent.SelectedItem;
                    var filteredData = Antecdent_list.Where(item =>
                      item.type_anteced.Nom_TypeANteced.Contains(type_anteced.Nom_TypeANteced)
                   );
                    if (filteredData.Count() > 0)
                    {
                        Border_All_antecdents.Visibility = Visibility.Visible;
                        border_image_antecdent.Visibility = Visibility.Collapsed;
                        myListView_antecedent.ItemsSource = filteredData;  //affecter le result a listview
                    }
                    else
                    {
                        Border_All_antecdents.Visibility = Visibility.Collapsed;
                        border_image_antecdent.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir fenetre de details ordonnace
        private void DetailsOrdonnnace_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_ordonnaces.SelectedItem != null)
                {
                    GestionOrdonnance_Class res = (GestionOrdonnance_Class)datagrid_ordonnaces.SelectedItem;
                    Details_Ordonnance f = new Details_Ordonnance(res);
                    f.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button : new consultation 
        private void NewConsult_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez vous Ajouter une nouvelle consultation ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    AddConsultation_Controle addconsult =
                                  new AddConsultation_Controle(idPatient, user);
                    Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                    if (acceuil != null)
                    {
                        acceuil.ChangerControleur(addconsult);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
     
        //button : modifer patient
        private void ModiferPatient_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ModiferPatient(idPatient, user).ShowDialog();
                loadPatinet();
                load_TypeAnteced();
                Load_Antecdents();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }


        }

        // button : modifer consultation : ouvrir controle Details_Consultation
        private void EditConsult_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voulez vous modifier cette consultation ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (datagrid_Consult.SelectedItem != null)
                    {
                        ConsultationClass c = (ConsultationClass)datagrid_Consult.SelectedItem;
                        Details_Consultation update_consult = new Details_Consultation(c.IdConsult, user);
                        Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                        if (acceuil != null)
                        {
                            acceuil.ChangerControleur(update_consult);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // img : load antecdents
        private void image_initialiser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                border_image_antecdent.Visibility = Visibility.Collapsed;
                Border_All_antecdents.Visibility = Visibility.Visible;
                myListView_antecedent.ItemsSource = Antecdent_list;
                datagrid_TypeANtecedent.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- END EVENTS   -------------------------------



        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        
       // ----------- start sql dependecy : Patient  -------------
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
                if ( (e.ChangeType != ChangeType.None) && (EntityPatient.id_patient == idPatient) )
                {
                    switch (e.ChangeType)
                    {
                        case ChangeType.Update:
                            Dispatcher.Invoke(() =>
                            {
                                loadPatinet();
                            });
                            break;

                        case ChangeType.Delete:
                            Dispatcher.Invoke(() =>
                            {
                                new Notification_window
                                                   (" Ce Patient a ete  supprimer")
                                                   .ShowDialog();
                                PatientControlle_Medecin p = new PatientControlle_Medecin(user);
                                Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                                if (acceuil != null)
                                {
                                    acceuil.ChangerControleur(p);
                                }
                            });
                            break;
                    }
                }
            }
            catch (Exception)
            { }
        }
        private void Stop_TablePatient_Dependency()
        {
            try
            {
                if (PatientSqlTableDependency != null)
                {
                    PatientSqlTableDependency.Stop();
                }
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
                if( (e.ChangeType != ChangeType.None) && (EntityAntecedents.patient_id == idPatient) )
                {
                    Dispatcher.Invoke(() =>
                    {
                         Load_Antecdents();
                        datagrid_TypeANtecedent.SelectedIndex = -1;
                    });
                } 
            }
            catch (Exception)
            { }
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


        // ----------- start sql dependecy : Consultation  -------------
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
                if (e.ChangeType != ChangeType.None)
                {
                    if (EntityConsultation.Patient_id == idPatient)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                    //load  consultations
                                    Consult_List = ConsultationClass.DisplayConsult_Datagrid(idPatient);
                                    datagrid_Consult.ItemsSource = null;
                                    if (Consult_List.Count > 0)
                                    {
                                        no_item.Visibility = Visibility.Collapsed;
                                        datagrid_Consult.ItemsSource = Consult_List;
                                        SelectionItemConsultation(IdConsultation);
                                    }
                                    else no_item.Visibility = Visibility.Visible;
                                });
                                break;

                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    // cas : update la consultation qui seletionner
                                    if (EntityConsultation.id_Consult == IdConsultation)
                                    {
                                        loadConsult(IdConsultation);
                                    }   
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    // cas : delete la consultation qui seletionner
                                    if (EntityConsultation.id_Consult == IdConsultation)
                                    {
                                        new Notification_window
                                                    (" cette Consultation a ete  supprimer ")
                                                    .ShowDialog();
                                        InitialiserChampsConsultation();
                                        loadAll_Consultation();
                                       
                                    }
                                    // cas : delete la consultation qui ne  seletionner  pas
                                    else
                                    {
                                        //load  consultations
                                        Consult_List = ConsultationClass.DisplayConsult_Datagrid(idPatient);
                                        datagrid_Consult.ItemsSource = null;
                                        if (Consult_List.Count > 0)
                                        {
                                            no_item.Visibility = Visibility.Collapsed;
                                            datagrid_Consult.ItemsSource = Consult_List;
                                            SelectionItemConsultation(IdConsultation);
                                        }
                                        else no_item.Visibility = Visibility.Visible;
                                    } 
                                });
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {}
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
                if (e.ChangeType != ChangeType.None)
                {
                    if( EntityBilans != null)
                    {
                        if( EntityBilans.Consult_id == IdConsultation)
                        {
                            if( (e.ChangeType == ChangeType.Delete) || (e.ChangeType == ChangeType.Insert) )
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    ExpanderBilans.Header = "Bilans " + "( " + BilansClass.NBr_Bilans_INConsultation(IdConsultation).ToString() + " )";
                                    loadAllBilan(IdConsultation);
                                });
                            }
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
                if (e.ChangeType != ChangeType.None)
                {
                    if (EntityOrdonnance != null)
                    {
                        if (EntityOrdonnance.Consult_id == IdConsultation)
                        {
                            if ( (e.ChangeType == ChangeType.Delete) || (e.ChangeType == ChangeType.Insert) )
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    ExpanderOrdonnace.Header = "Ordonnnace " + "( " + Ordonnace_Class.NBr_Ordonnnace_INConsultation(IdConsultation).ToString() + " )";
                                    loadAllOrdonnance(IdConsultation);
                                });
                            }
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
       
        private void InitialiserChampsConsultation()
        {
            TitreConsult.Text = "";
            motif_Consult.Text = "";
            ExamenC_consult.Text = "";
            Diagnostique_Consult.Text = "";
            Taille_Patient.Text = "";
            Poids_patient.Text = "";
            TerminerConsult.Text = "";
            Expander_InfoConsult.IsExpanded = false;
            ExpanderBilans.IsExpanded = false;
            ExpanderOrdonnace.IsExpanded = false;
        }
       
        //afficher info d une  Consultation en datagrid
        private void Show_Info_Select_Consult()
        {
            try
            {
                if (datagrid_Consult.SelectedItem != null)
                {
                    ConsultationClass c = (ConsultationClass)datagrid_Consult.SelectedItem;
                    IdConsultation = c.IdConsult;
                    loadConsult(c.IdConsult);
                    loadAllBilan(c.IdConsult);
                    loadAllOrdonnance(c.IdConsult);
                    Expander_InfoConsult.IsExpanded = true;
                    ExpanderBilans.Header = "Bilans " + "( " + BilansClass.NBr_Bilans_INConsultation(c.IdConsult).ToString() + " )";
                    ExpanderOrdonnace.Header = "Ordonnnace " + "( " + Ordonnace_Class.NBr_Ordonnnace_INConsultation(c.IdConsult).ToString() + " )";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        //seul Admin peut supprimer  patient
        private void Droits_user()
        {
            try
            {
                if (user.IsAdmin == 0) Delete_btn.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        //data fiche patient
         private void Load_Fiche_Patient()
        {
            try 
            { 
                loadPatinet();
                loadAll_Consultation();
                load_TypeAnteced();
                Load_Antecdents();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
      
        //load antecdents
        private void Load_Antecdents()
        {
            try
            { 
                Antecdent_list = Gestion_Antecedent.DisplayAnteced_OfPatient(idPatient);
                if (Antecdent_list.Count > 0)
                {
                    myListView_antecedent.ItemsSource = Gestion_Antecedent.DisplayAnteced_OfPatient(idPatient);
                    Border_All_antecdents.Visibility = Visibility.Visible;
                    border_image_antecdent.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Border_All_antecdents.Visibility = Visibility.Collapsed;
                    border_image_antecdent.Visibility = Visibility.Visible;
                }
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
                Patient_Name.Text = p.NomPatient + " " + p.PrenomPatient;
                Nom_patient.Text = p.NomPatient;
                prenom_patient.Text = p.PrenomPatient;
                Cin_Patient.Text = p.CINPatient;
                DateNaissance.Text = p.DateNaissancePatient.ToShortDateString();
                LieuNiassance.Text = p.LieuNaissancePatient;
                Age_Patient.Text = p.AgePatient.ToString();
                Phone_patient.Text = p.PhonePatient;
                Email_patient.Text = p.EmailPatient;
                Date_Save_Patient.Text = p.DateSavePatient.ToShortDateString();
                Address_patient.Text = p.AddressPatient;
                Profession_patient.Text = p.ProfessPatient;
                Groupsang_patient.Text = GroupSangClass.NamofGroupSang(Convert.ToInt32(p.GroupSangPatient));
                Assurance_patient.Text = AssuranceClass.NamofAssurance(Convert.ToInt32(p.AssurancePatient));
                etatCivil_patient.Text = p.EtatCivilPatient;
                if (p.SexPatient == 'H')
                {
                    Sex_Patient.Text = "Homme";
                }
                else Sex_Patient.Text = "Femme";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load consultations
        private void loadAll_Consultation()
        {
            try 
            { 
                Consult_List = ConsultationClass.DisplayConsult_Datagrid(idPatient);
                datagrid_Consult.ItemsSource = null;
                if (Consult_List.Count > 0)
                {
                    no_item.Visibility = Visibility.Collapsed;
                    datagrid_Consult.ItemsSource = Consult_List;
                    datagrid_Consult.SelectedIndex = 0;
                    Show_Info_Select_Consult();
                }
                else no_item.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M200 !!");
            }
        }

        //load consultation
        private void loadConsult(int idc)
        {
            try
            { 
                ConsultationClass c = ConsultationClass.SelectConsultation(idc);
                TitreConsult.Text = "Consultation : " + c.DateConsult.ToShortDateString();
                motif_Consult.Text = c.MotifConsult;
                ExamenC_consult.Text = c.ExamenClinque_Consult;
                Diagnostique_Consult.Text = c.Diagnostique_Consult;
                if (c.Taille_patient == 0)
                {
                    Taille_Patient.Text = "";
                }else Taille_Patient.Text = c.Taille_patient.ToString();

                if (c.Poids_patient == 0)
                {
                    Poids_patient.Text = "";
                } else  Poids_patient.Text = c.Poids_patient.ToString();
           
                if (c.Terminer_consult == 1)
                {
                    TerminerConsult.Text = "cachtée";
                }
                else
                    TerminerConsult.Text = " N'est Pas cachtée";
            }
            catch (Exception )
            {
                MessageBox.Show("Opération d'entrée innatendu N A002 !!");
            }
        }
        private void SelectionItemConsultation(int IDCons)
        {
            foreach (ConsultationClass item in datagrid_Consult.Items)
            {
                if (item.IdConsult == IDCons)
                {
                    datagrid_Consult.SelectedIndex = datagrid_Consult.Items.IndexOf(item);
                    break;
                }
            }
        }
      
        //load bilans of consultation
        private void loadAllBilan(int idc)
        {
            try
            { 
                 datagrid_Bilans.ItemsSource = GestionBilan_class.DisplayResultsBilans_Datagrid(idc);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load ordonnacement of consultation
        private void loadAllOrdonnance(int idc)
        {
            try 
            { 
                  datagrid_ordonnaces.ItemsSource = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(idc);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
     
        //load type intecdent
        private void load_TypeAnteced()
        {
            try 
            { 
                 datagrid_TypeANtecedent.ItemsSource = TypeAntecedent.DisplayTypeAnteced();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
