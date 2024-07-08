using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Patient_Controlle.xaml
    /// </summary>
    public partial class PatientControlle_Medecin : UserControl
    {
        public Utilisateur_Class user;
        ObservableCollection<PatientClass> ListPatient;  // list qui recupere les donnes de patients
     
        public SqlTableDependency<DMPatient> PatientSqlTableDependency;
        public PatientControlle_Medecin(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                loadDataGrid();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
       
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

        // ouvrir fiche patient
        private void EditPatient_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagridPatients.SelectedItem != null)
                {
                    PatientClass p = (PatientClass)datagridPatients.SelectedItem;
                    Fiche_Patient_Controlle addconsult =
                       new Fiche_Patient_Controlle(p.IDPatient, user);
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
                        item.PhonePatient.Contains(searchTerm) ||
                      item.SexPatient.ToString().ToUpper().Contains(searchTerm) ||
                      item.AssurancePatient.ToUpper().Contains(searchTerm)
               // add more conditions for each property you want to search
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

        // remplir data grid veiw
        private void loadDataGrid()
        {
            try 
            { 
                ListPatient = PatientClass.DisplayPatinet_Datagrid();
                datagridPatients.ItemsSource = ListPatient;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES    ------------------------------------ ---

    }
}
