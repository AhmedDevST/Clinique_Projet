using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using System.Threading.Tasks;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Antecdents_Window.xaml
    /// </summary>
    public partial class Antecdents_Window : Window
    {
        public int Patient_ID;

        public Utilisateur_Class user;

        ObservableCollection<Gestion_Antecedent> List_Antecedents = new ObservableCollection<Gestion_Antecedent>();
       
        public Gestion_Antecedent? ObjAnteced = null; //cette objet pour affichage antecednet

        public SqlTableDependency<DMAntecedents> AntecedentsSqlTableDependency;

        public SqlTableDependency<DMPatient> PatientSqlTableDependency;

        public Antecdents_Window(int idPatient,Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Patient_ID = idPatient;
                Name_patient.Text = PatientClass.SelectNomPatient(Patient_ID);
                loadType_Antecedent();
                LoadData_Antecedents();
                EditAnteced_btn.IsEnabled = false;
                DeleteAnteced_btn.IsEnabled = false;
                this.user = user;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }




    // -----------------------  SATRT  EVENTS :  ------------------------------------
        //close window
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    Task.Run(() => Stop_TablePatient_Dependency()),
                    Task.Run(() => Stop_TableAntecedents_Dependency())
                    );
            }
            catch (Exception)
            {
            }
        }

        //open window 
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    Task.Run(() => Start_TablePatient_Dependency()) ,
                    Task.Run(() => Start_TableAntecedents_Dependency()) 
                );
            }
            catch (Exception)
            {
            }
        }
        
        //button : annuler
        private void btn_annuler_antecedent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Voullez vous sur de quitter ?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

        //ajouter   antecedent
        private void AddAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_TypeAnteced;
                if (comboboxType_Anteced.SelectedItem != null)
                {
                    id_TypeAnteced = Convert.ToInt32(comboboxType_Anteced.SelectedValue.ToString());

                    // tester si le type Antecedents  n est pas deja existe
                    if (!List_Antecedents.Any(inteced => inteced.anteced.IDType_Anteced == id_TypeAnteced))
                    {
                        // confirmation
                        MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Antécedent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            // Ajouter dans base de donnee
                            Antecedents Anteced = new Antecedents(Patient_ID, id_TypeAnteced, DateTime.Now, Description_Anteced.Text);
                            Anteced.AddAntecedent();
                            //sql dependecy va Ajouter  dans la list des Antecedents 
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

        //modifier antecedents
        private void EditAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // confirmation
                MessageBoxResult res = MessageBox.Show("Voullez vous Ajouter cet Antécedent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjAnteced != null)
                    {
                        //modifier dans la base de donnee
                        Antecedents Anteced = new Antecedents(Patient_ID, ObjAnteced.anteced.IDType_Anteced, Description_Anteced.Text);
                        Anteced.UpdateAntecedent();
                       //sql dependecy va modifier dans la list des Antecedents                       
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
                MessageBoxResult res = MessageBox.Show("Voullez vous sur d'effectue cette operation?", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (ObjAnteced != null)
                    {
                        //delete dans la base de donnee
                        Antecedents.DeleteAntecedent(Patient_ID, ObjAnteced.anteced.IDType_Anteced);
                         //sql dependecy va actualiser le datagrid    
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

        //ouvrir fenetre de parametre d' antecedent
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

        //annuler antecedents : initiliser les champs 
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


        // -----------------------  END  EVENTS ------------------------------------


        // -----------------------  SATRT  METHODES  :  ------------------------------------
       
        // afficher un  image si on a pas des antecdents au list
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
                List_Antecedents = Gestion_Antecedent.DisplayAnteced_OfPatient(Patient_ID);
                datagrid_Antecedent.ItemsSource = List_Antecedents;
                show_image_aucun_antecdents();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //load combobox  de type_Antecedents
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

        // iniasilaiser les champs  : ObjAnteced,datagrid , Description ,  combobox , button 
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

        //selectionner un element au data grid par id de type Antecedant 
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



        // ----------- start sql dependecy : Patient : le cas de suppression   -------------
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
            catch (Exception )
            {

            }
        }
        private void Changed_Patient_Dependency(object sender, RecordChangedEventArgs<DMPatient> e)
        {
            try
            {
                DMPatient EntityPatient = e.Entity;
                if ((e.ChangeType == ChangeType.Delete) && (EntityPatient.id_patient == Patient_ID))
                {
                    Dispatcher.Invoke(() =>
                    {
                        new Notification_window(" Cet Patient  a ete  supprimer  ")
                                                       .ShowDialog();
                        Close();
                    });
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

        // ----------- start sql dependecy : Antecedents  -------------
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
                    if (EntityAntecedents.patient_id == Patient_ID)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                     List_Antecedents.Add(new Gestion_Antecedent
                                     {
                                         anteced = new Antecedents { IDType_Anteced = EntityAntecedents.TypeAtecd_id , Date_Anteced = EntityAntecedents.Date_Anteced , Descrip_Anteced = EntityAntecedents.Descrip_Antecedent },
                                         type_anteced = new TypeAntecedent { Nom_TypeANteced = TypeAntecedent.SelectNAme_TypeAnteced(EntityAntecedents.TypeAtecd_id) },
                                     });

                                    datagrid_Antecedent.ItemsSource = null;
                                    datagrid_Antecedent.ItemsSource = List_Antecedents;
                                    show_image_aucun_antecdents();

                                    if (ObjAnteced != null)
                                    {
                                        SelectionItemAntecedant(ObjAnteced.anteced.IDType_Anteced);
                                    }
                                    MessageBox.Show("un Antecedant est ajoutée ");
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
                                            MessageBox.Show(" un Antecedent est  modifiée ");
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
                                    MessageBox.Show("antecdents est  supprimée ");
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
                if (AntecedentsSqlTableDependency != null)
                {
                    AntecedentsSqlTableDependency.Stop();
                }
            }
            catch (Exception)
            {

            }
        }

        // ----------- End sql dependecy : Antecedents -------------

       // -----------------------  END  METHODES  :  ------------------------------------
    }
}
