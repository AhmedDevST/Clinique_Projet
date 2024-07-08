using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using Clinique_Projet.validation;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ValidationResult = FluentValidation.Results.ValidationResult;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.connectionDb;
using System.Threading.Tasks;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Details_Consultation.xaml
    /// </summary>
    public partial class Details_Consultation : UserControl
    {
        public static int ConsultID;
        public int Patient_ID;
        public int IsTerminer;
        public decimal poids = 0;
        public int taille = 0;
        public Utilisateur_Class user;
        public SqlTableDependency<DMBilans> BilansSqlTableDependency;
        public SqlTableDependency<DMOrdonnance> OrdonnanceSqlTableDependency;
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;
        public SqlTableDependency<DMAntecedents> AntecedentsSqlTableDependency;
        public SqlTableDependency<DMProchaine_RDV> Prochaine_RDVSqlTableDependency;
        public Details_Consultation(int idconsult,Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                ConsultID = idconsult;
                load_combobox_prpositions();
                loadInfo_consultation();
                initiliser_antecdents();
                load_Suivi();
                this.user = user;
                Droits_user();
                Initiliser_counts();
            }
            catch (Exception)
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
                   Task.Run(() => Stop_TableAntecedents_Dependency()),
                   Task.Run(() => Stop_TableConsultation_Dependency()),
                   Task.Run(() => Stop_TableBilans_Dependency()),
                   Task.Run(() => Stop_TableOrdonnance_Dependency()),
                   Task.Run(() => Stop_TableProchaine_RDV_Dependency())
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
                    Task.Run(() => Start_TableAntecedents_Dependency()),
                    Task.Run(() => Start_TableConsultation_Dependency()),
                    Task.Run(() => Start_TableBilans_Dependency()),
                    Task.Run(() => Start_TableOrdonnance_Dependency()),
                    Task.Run(() => Start_TableProchaine_RDV_Dependency())
                );
            }
            catch (Exception)
            {
            }
        }

        //save info consultation
        private void save_btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //save in database
                if (Valide_Data())
                {
                    // si les donnes est valide
                    MessageBoxResult res = MessageBox.Show("Voullez vous Enregistrer ces modifications ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (Update_Data())
                        {
                            new succes().ShowDialog();

                            // add notification
                            Notification_class notification_ =
                            new Notification_class(0, user.id_user, ConsultID, "cons", DateTime.Now, " Modifié les informations de consultation ");
                            notification_.Add_Notification();
                        }
                        else MessageBox.Show("Opération d'entrée innatendu N if500 !!");

                        //load rapport 
                        load_consultation_suivi();
                        //disabled the textbox
                        annuler_edit_info();
                    }
                }
                else MessageBox.Show("les données ne sont pas valides");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M304!!");
            }
        }

        //annuler btn
        private void annuler_btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                loadInfo_consultation();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M708!!");
            }
        }

        //enable edit info consultation
        private void edit_btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                motif_patient.IsReadOnly = false;
                RemarqueMotif.Visibility = Visibility.Visible;
                examen_clinique.IsReadOnly = false;
                RemarqueExamen_Clinique.Visibility = Visibility.Visible;
                Diagnostique_medical.IsReadOnly = false;
                RemarqueDiagnostique.Visibility = Visibility.Visible;
                Poids_patient.IsReadOnly = false;
                RemarquePoids.Visibility = Visibility.Visible;
                Taille_patient.IsReadOnly = false;
                RemarqueTaille.Visibility = Visibility.Visible;
                Terminer_checkBox.IsEnabled = true;
                edit_btn.Visibility = Visibility.Collapsed;
                border_save_btn.Visibility = Visibility.Visible;
                if (combobox_propositions.Items.Count > 0) border_combobx_proposition.Visibility = Visibility.Visible;
                img_add_diagnostique.Visibility = Visibility.Visible;
                img_add_motif.Visibility = Visibility.Visible;
                img_add_Examen.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M982!!");
            }
        }

        //supprimer consultation
        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // si les donnes est valide
                MessageBoxResult res = MessageBox.Show("Vous voullez Sûr de supprimer cette consultation", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    ConsultationClass.DeleteConsultation(ConsultID);
                    new succes().ShowDialog();
                    Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                    if (acceuil != null)
                    {
                        acceuil.ChangerControleur(new GestionConsultation_Control(user));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendue N M234 !!");
            }
        }

        // imprimer ordonnoce
        private void btn_imprimer_ordonnonce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Imprimer_Ordonnnace(ConsultID, Patient_ID).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendue N M523 !!");
            }
        }

        //imprimer analyse
        private void btn_imprimer_analyse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new imprimer_analyses(ConsultID, Patient_ID).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir le fenetre de gestion des  analyses de consultation
        private void btn_analyses_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Analyse_Consultation(ConsultID, user).ShowDialog();
                load_list_analyse_suivi(); //load analyse in rapport
                count_analyse.Text = "( " + BilansClass.NBr_Bilans_INConsultation(ConsultID) + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir le fenetre de gestion des ordonnonces de consultation 
        private void btn_ordonnonce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Ordonnoce_consultation(ConsultID, user).ShowDialog();
                load_list_ordonnonce_suivi(); //load ordonnonce rapport
                count_ordonnonce.Text = "( " + Ordonnace_Class.NBr_Ordonnnace_INConsultation(ConsultID) + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir fenetre de gestion des prochaine rendez vous de consultation 
        private void btn_rdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Prochaine_RDV(Patient_ID, ConsultID).ShowDialog();
                load_list_Prdv_suivi();
                count_prdv.Text = "(" + Prochaine_RDV_class.Count_PRDV(ConsultID) + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrire fenetre de gestion des  antecedents
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

        //ouvrire fenetre de  cree  un certificat
        private void btn_certificat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Certificate_medical_window(Patient_ID).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //valide motif
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

        //valide  poids de patient
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

        //valide  taille de patient
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
        
        // les propositions
        private void combobox_propositions_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            try
            {
                string searchTerm = combobox_propositions.Text;
                ObservableCollection<string> list = ConsultationClass.Display_champs_consultation();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var filteredItems = list.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();

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

        // ajouter  propositions au champs motif 
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

        // ajouter  propositions au champs diagnostique 
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

        // ajouter  propositions au champs Examen clinique  
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

        private void annuler_edit_info()
        {
            try
            {
                motif_patient.IsReadOnly = true;
                RemarqueMotif.Visibility = Visibility.Collapsed;
                examen_clinique.IsReadOnly = true;
                RemarqueExamen_Clinique.Visibility = Visibility.Collapsed;
                Diagnostique_medical.IsReadOnly = true;
                RemarqueDiagnostique.Visibility = Visibility.Collapsed;
                Poids_patient.IsReadOnly = true;
                RemarquePoids.Visibility = Visibility.Collapsed;
                Taille_patient.IsReadOnly = true;
                RemarqueTaille.Visibility = Visibility.Collapsed;
                Terminer_checkBox.IsEnabled = false;
                edit_btn.Visibility = Visibility.Visible;
                border_save_btn.Visibility = Visibility.Collapsed;
                border_combobx_proposition.Visibility = Visibility.Collapsed;
                img_add_diagnostique.Visibility = Visibility.Collapsed;
                img_add_motif.Visibility = Visibility.Collapsed;
                img_add_Examen.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M542 !!");
            }
        }

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
                if ((e.ChangeType != ChangeType.None) && (EntityAntecedents.patient_id == Patient_ID))
                {
                    Dispatcher.Invoke(() =>
                    {
                        initiliser_antecdents();
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
                    if (EntityConsultation.Patient_id == Patient_ID)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    loadInfo_consultation();
                                    load_consultation_suivi();
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    new Notification_window
                                                    (" cette Consultation a ete  supprimer ")
                                                    .ShowDialog();
                                    Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                                    if (acceuil != null)
                                    {
                                        acceuil.ChangerControleur(new GestionConsultation_Control(user));
                                    }
                                });
                                break;
                        }
                    }
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
                if (e.ChangeType != ChangeType.None)
                {
                    if (EntityBilans != null)
                    {
                        if (EntityBilans.Consult_id == ConsultID)
                        {
                            //les cas : insert ou delete
                            if( e.ChangeType != ChangeType.Update)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    count_analyse.Text = "(" + BilansClass.NBr_Bilans_INConsultation(ConsultID) + ")";
                                });
                            }
                            // tous cas 
                            Dispatcher.Invoke(() =>
                            {
                                  load_list_analyse_suivi();
                            });
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
                        if (EntityOrdonnance.Consult_id == ConsultID)
                        {
                            //les cas : insert ou delete
                            if (e.ChangeType != ChangeType.Update)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    count_ordonnonce.Text = "(" + Ordonnace_Class.NBr_Ordonnnace_INConsultation(ConsultID) + ")";
                                });
                            }
                            //les tous cas 
                            Dispatcher.Invoke(() =>
                            {
                                load_list_ordonnonce_suivi();
                            });
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
    
        // ----------- start sql dependecy : Prochaine_RDV  -------------

        private void Start_TableProchaine_RDV_Dependency()
        {
            try
            {
                if (Prochaine_RDVSqlTableDependency == null)
                {
                    Prochaine_RDVSqlTableDependency = new SqlTableDependency<DMProchaine_RDV>(ConnectDb.GetConnectionstring(), "Prochaine_RDV");
                    Prochaine_RDVSqlTableDependency.OnChanged += Changed_Prochaine_RDV_Dependency;
                    Prochaine_RDVSqlTableDependency.Start();
                }
            }
            catch (Exception)
            {
            }
        }
        private void Changed_Prochaine_RDV_Dependency(object sender, RecordChangedEventArgs<DMProchaine_RDV> e)
        {
            try
            {
                DMProchaine_RDV EntityProchaine_RDV = e.Entity;
                if (e.ChangeType != ChangeType.None)
                {
                    if (EntityProchaine_RDV != null)
                    {
                        if (EntityProchaine_RDV.Consult_id == ConsultID)
                        {
                            //les cas : insert ou delete
                            if (e.ChangeType != ChangeType.Update)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    count_prdv.Text = "(" + Prochaine_RDV_class.Count_PRDV(ConsultID) + ")";
                                });
                            }
                            //les tous cas 
                            Dispatcher.Invoke(() =>
                            {
                                load_list_Prdv_suivi();
                            });
                        }
                    }
                }

            }
            catch (Exception)
            { }
        }
        private void Stop_TableProchaine_RDV_Dependency()
        {
            try
            {
                Prochaine_RDVSqlTableDependency?.Stop();
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy :Antecdents  -------------

       //seul admin peut supprimer la consultation
        private void Droits_user()
        {
            if (user.IsAdmin == 0) delete_btn.Visibility = Visibility.Collapsed;
        }

       // ------------- suivi de  consultation ------------------

        //load consultation suivi
        private void load_consultation_suivi()
        {
            try
            {
                //load info consultation
                ConsultationClass consult = ConsultationClass.SelectConsultation(ConsultID);
                Date_consult_suivi.Text = consult.DateConsult.ToString("dddd dd MMMM yyyy ", new CultureInfo("fr-FR"));
                motfi_suivi.Text = consult.MotifConsult;
                Examen_clinique_suivi.Text = consult.ExamenClinque_Consult;
                Diagnostique_suivi.Text = consult.Diagnostique_Consult;
                if (consult.Poids_patient == 0)
                {
                    Poids_suivi.Text = "";
                }
                else Poids_suivi.Text = consult.Poids_patient.ToString() + " KG";
                if (consult.Taille_patient == 0)
                {
                    Taille_suivi.Text = "";
                }
                else Taille_suivi.Text = consult.Taille_patient.ToString() + " Cm";

                // consultation cachtee
                if (IsTerminer == 1)
                {
                    Cachtee.Text = "La Consultation est cachtée";
                    Cachtee.Foreground = Brushes.Green;
                }
                else
                {
                    Cachtee.Text = "La Consultation est Non cachtée";
                    Cachtee.Foreground=Brushes.Red;
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M989 !!");
            }
        }

        //load le rapport de consultation
        private void load_Suivi()
        {
            try
            {
                //load consultation info
                load_consultation_suivi();

                //nom patient
                PatientClass p = PatientClass.SelectPatient(Patient_ID);
                if (p != null)
                {
                    if (p.SexPatient == 'F')
                    {
                        name_patient_suivi.Text = "Mm" + " " + p.NomPatient + " " + p.PrenomPatient;
                    }
                    else name_patient_suivi.Text = "Mr" + " " + p.NomPatient + " " + p.PrenomPatient;
                }

                //load ordonnonce 
                load_list_ordonnonce_suivi();

                //load analyse 
                load_list_analyse_suivi();

                //lod prdv
                load_list_Prdv_suivi();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load list des analyses de consultation
        private void load_list_analyse_suivi()
        {
            try
            {
                ObservableCollection<GestionBilan_class> List = GestionBilan_class.DisplayResultsBilans_Datagrid(ConsultID);
                myListView_analyse_suivi.ItemsSource = null;
                nombre_analyse_suivi.Text="("+BilansClass.NBr_Bilans_INConsultation(ConsultID)+")";
                if (List.Count <= 0)
                {
                    myListView_analyse_suivi.Visibility = Visibility.Collapsed;
                    border_image_Aucun_analyse.Visibility = Visibility.Visible;
                }
                else
                {
                    myListView_analyse_suivi.Visibility = Visibility.Visible;
                    myListView_analyse_suivi.ItemsSource = List;
                    border_image_Aucun_analyse.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load list des ordonnonces de consultation
        private void load_list_ordonnonce_suivi()
        {
            try
            {
                ObservableCollection<GestionOrdonnance_Class> List = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(ConsultID);
                myListView_Ordonnonce_suivi.ItemsSource = null;
                nombre_ordonnonce_suivi.Text="("+Ordonnace_Class.NBr_Ordonnnace_INConsultation(ConsultID)+")";
                if (List.Count <= 0)
                {
                    myListView_Ordonnonce_suivi.Visibility = Visibility.Collapsed;
                    border_image_Aucun_ordonnoces.Visibility = Visibility.Visible;
                }
                else
                {
                    myListView_Ordonnonce_suivi.Visibility = Visibility.Visible;
                    myListView_Ordonnonce_suivi.ItemsSource = List;
                    border_image_Aucun_ordonnoces.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M787 !!");
            }
        }

        //load list des prdv de consultation
        private void load_list_Prdv_suivi()
        {
            try
            {
                ObservableCollection<RendezVous> List = Prochaine_RDV_class.Display_PRDV(ConsultID);
                myListView_Prdv_suivi.ItemsSource = null;
                nombre_prdv_suivi.Text="("+Prochaine_RDV_class.Count_PRDV(ConsultID) +")";
                if (List.Count <= 0)
                {
                    myListView_Prdv_suivi.Visibility = Visibility.Collapsed;
                    border_image_Aucun_prdv.Visibility = Visibility.Visible;
                }
                else
                {
                    myListView_Prdv_suivi.Visibility = Visibility.Visible;
                    myListView_Prdv_suivi.ItemsSource = List;
                    border_image_Aucun_prdv.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //------------------ consultation info -------------

        private void Initiliser_counts()
        {
            count_analyse.Text = "(" + BilansClass.NBr_Bilans_INConsultation(ConsultID) + ")";
            count_ordonnonce.Text = "(" + Ordonnace_Class.NBr_Ordonnnace_INConsultation(ConsultID) + ")";
            count_prdv.Text = "(" + Prochaine_RDV_class.Count_PRDV(ConsultID) + ")";
        }

        //load consultation info
        private void loadInfo_consultation()
        {
            try
            {
                ConsultationClass consult = ConsultationClass.SelectConsultation(ConsultID);
                DateConsultation.Text = consult.DateConsult.ToString("dddd dd MMMM yyyy ", new CultureInfo("fr-FR"));
                motif_patient.Text = consult.MotifConsult;
                examen_clinique.Text = consult.ExamenClinque_Consult;
                Diagnostique_medical.Text = consult.Diagnostique_Consult;

                if (consult.Poids_patient == 0)
                {
                    Poids_patient.Text = "";
                }
                else Poids_patient.Text = consult.Poids_patient.ToString();
                if (consult.Taille_patient == 0)
                {
                    Taille_patient.Text = "";
                }
                else Taille_patient.Text = consult.Taille_patient.ToString();

                Patient_ID = consult.ID_patient;
                IsTerminer = consult.Terminer_consult;
                initialiser_ChekBox_terminer();
                //nom patient
                PatientClass p = PatientClass.SelectPatient(Patient_ID);
                if (p != null)
                {
                    if (p.SexPatient == 'F')
                    {
                        Nom_patient.Text = "Mme " + p.NomPatient + " " + p.PrenomPatient;
                    }
                    else Nom_patient.Text = "Mr " + p.NomPatient + " " + p.PrenomPatient;
                }
                annuler_edit_info();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
      
        //initiliser checkbox 
        private void initialiser_ChekBox_terminer()
        {
            try
            {
                if (IsTerminer == 1)
                {
                    Terminer_checkBox.IsChecked = true;
                }
                else
                {
                    Terminer_checkBox.IsChecked = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

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
                MessageBox.Show("Opération d'entrée innatendu N M893 !!");
            }
        }

        private void load_combobox_prpositions()
        {
            try
            {
                ObservableCollection<string> list = ConsultationClass.Display_champs_consultation();
                if (list.Count > 0)
                {
                    combobox_propositions.ItemsSource = list;
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
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M652 !!");
            }
        }

        // update data in database
        private bool Update_Data()
        {
            try
            {
                Terminer_Consult();
                // update the consultation
                ConsultationClass consult = new ConsultationClass(ConsultID, motif_patient.Text, DateTime.Now, examen_clinique.Text,
                                                        Diagnostique_medical.Text, poids,
                                                        taille, IsTerminer, Patient_ID);
                consult.UpdateConsultation();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // if consult termienr ou non
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
                MessageBox.Show("Opération d'entrée innatendu N M457 !!");
            }       
        }

        /// ----------- fonction de validation -----------

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
                if (Validation_function(Taille_patient, RemarqueTaille, 7))
                {
                    taille = Convert.ToInt32(Taille_patient.Text);
                }
                else bTaille = false;
            }

            return bPoids && bTaille && b2 ;
        }

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
