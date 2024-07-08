using Clinique_Projet.Controlers;
using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.connectionDb;
namespace Clinique_Projet
{
    /// <summary>
    /// Logique d'interaction pour GestionConsultation_Control.xaml
    /// </summary>
    public partial class consultation_sec : UserControl
    {
        ObservableCollection<GestionConsultation_Class> ListConsultation;  // list qui recupere les donnes de consultaton
        
        public Utilisateur_Class user;

        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;

        public consultation_sec(Utilisateur_Class user)
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Start_TableConsultation_Dependency();
            }
            catch (Exception)
            {
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Stop_TableConsultation_Dependency();
            }
            catch (Exception)
            {
            }
        }

        //search
        private void mytext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchTerm = mytext.Text.ToUpper();   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = ListConsultation.Where
                        (item =>
                            item.Patient.NomPatient.ToUpper().Contains(searchTerm) ||
                            item.Patient.PrenomPatient.ToUpper().Contains(searchTerm)
                        );
                    datagrid_Consult.ItemsSource = filteredData;  //affecter le result a datagrid
                }
                else datagrid_Consult.ItemsSource = ListConsultation; //si le textbox est vide on loaddatagrid
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //edit les bilans
        private void EditConsult_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Consult.SelectedItem != null)
                {
                    GestionConsultation_Class consult = (GestionConsultation_Class)datagrid_Consult.SelectedItem;

                    BIlans_Controlle_secritaire updateConsult = new BIlans_Controlle_secritaire(consult.consultation.IdConsult, user);

                    Acceuil acceuil = Window.GetWindow(this) as Acceuil;
                    if (acceuil != null)
                    {
                        acceuil.ChangerControleur(updateConsult);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        // -------------------------- END EVENTS  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

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
                if (e.ChangeType != ChangeType.None)
                {
                    Dispatcher.Invoke(() =>
                    {
                        loadDataGrid();
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
       
        private void loadDataGrid()
        {
            ListConsultation = GestionConsultation_Class.DisplayAll_Consultation();
            datagrid_Consult.ItemsSource = ListConsultation;
        }

        //-----------------------  END  METHODES   ------------------------------------ ----

    }
}
