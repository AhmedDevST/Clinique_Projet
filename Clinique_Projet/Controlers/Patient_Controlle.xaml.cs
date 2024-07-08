using Clinique_Projet.connectionDb;
using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.DatabaseModels;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Patient_Controlle.xaml
    /// </summary>
    public partial class Patient_Controlle : UserControl
    {
        public  ObservableCollection<PatientClass> ListPatient;  // list qui recupere les donnes de patients
        public Utilisateur_Class user;
        public SqlTableDependency<DMPatient> PatientSqlTableDependency;
        public Patient_Controlle(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                loadDataGrid();
                this.user = user;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        // load user controle 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Start_TablePatient_Dependency();
            }
            catch (Exception)
            {
            }
        }

        // Unload user controle 
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Stop_TablePatient_Dependency();
            }
            catch (Exception)
            {
            }
        }

        //ouvrir window nouveau patient
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Nouveau_Patient(user).ShowDialog();
                loadDataGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir modifier patient
        private void EditPatient_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagridPatients.SelectedItem != null)
                {
                    PatientClass p = (PatientClass)datagridPatients.SelectedItem;
                    new ModiferPatient(p.IDPatient, user).ShowDialog();
                    mytext.Text = "";
                    loadDataGrid();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //search in data grid view
        private void mytext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                search.BorderBrush = Brushes.Blue;
                string searchTerm = mytext.Text.ToUpper();   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = ListPatient.Where(item =>
                          item.CINPatient.ToUpper().Contains(searchTerm) ||
                          item.AgePatient.ToString().Contains(searchTerm) ||
                          item.NomPatient.ToUpper().Contains(searchTerm) ||
                          item.PrenomPatient.ToUpper().Contains(searchTerm) ||
                          item.PhonePatient.ToUpper().Contains(searchTerm) ||
                          item.SexPatient.ToString().ToUpper().Contains(searchTerm) ||
                          item.AssurancePatient.ToUpper().Contains(searchTerm)
                 );
                    datagridPatients.ItemsSource = filteredData;  //affecter le result a datagrid
                }
                else datagridPatients.ItemsSource = ListPatient; //si le textbox est vide on loaddatagrid
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        // remplir data grid veiw
        private void loadDataGrid()
        {
            ListPatient = PatientClass.DisplayPatinet_Datagrid();
            datagridPatients.ItemsSource = ListPatient;
        }
       
        // ----------- start sql dependecy -------------
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
                if (e.ChangeType != ChangeType.None)
                {
                    DMPatient EntityPatient = e.Entity;
                    if (EntityPatient != null)
                    {
                        switch (e.ChangeType)
                        {
                            case ChangeType.Insert:
                                Dispatcher.Invoke(() =>
                                {
                                    ListPatient.Add(PatientClass.SelectPatient(EntityPatient.id_patient));

                                    datagridPatients.ItemsSource = null;
                                    datagridPatients.ItemsSource = ListPatient;

                                });
                                break;

                            case ChangeType.Update:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (PatientClass item in ListPatient)
                                    {
                                        if (item.IDPatient == EntityPatient.id_patient)
                                        {
                                            item.NomPatient = EntityPatient.Nom_patient;
                                            item.PrenomPatient = EntityPatient.Prenom_patient;
                                            item.CINPatient = EntityPatient.CIN_patient;
                                            item.AgePatient = EntityPatient.Age_patient;
                                            item.PhonePatient = EntityPatient.Phone_patient;
                                            item.SexPatient = EntityPatient.Sex;
                                            item.AssurancePatient = AssuranceClass.NamofAssurance(EntityPatient.Assurance_id);
                                            datagridPatients.ItemsSource = null;
                                            datagridPatients.ItemsSource = ListPatient;
                                            return;
                                        }
                                    }
                                });
                                break;

                            case ChangeType.Delete:
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (PatientClass item in ListPatient)
                                    {
                                        if (item.IDPatient == EntityPatient.id_patient)
                                        {
                                            ListPatient.Remove(item);
                                            return;
                                        }
                                    }

                                    datagridPatients.ItemsSource = null;
                                    datagridPatients.ItemsSource = ListPatient;
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

        // ----------- End sql dependecy -------------

        //-----------------------  END  METHODES   ------------------------------------ ---
    }
}
