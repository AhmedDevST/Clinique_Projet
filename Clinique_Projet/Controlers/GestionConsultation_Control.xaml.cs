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

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour GestionConsultation_Control.xaml
    /// </summary>
    public partial class GestionConsultation_Control : UserControl
    {
        ObservableCollection<GestionConsultation_Class> ListConsultation;  // list qui recupere les donnes de consultations
        public Utilisateur_Class user;
        public SqlTableDependency<DMConsultation> ConsultationSqlTableDependency;

        //constructeur 
        public GestionConsultation_Control(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                loadDataGrid();
                this.user = user;       
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
                Stop_TableConsultation_Dependency();
            }
            catch (Exception)
            {
            }
        }

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

        //chercher
        private void mytext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchTerm = mytext.Text.ToUpper();   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = ListConsultation.Where(item =>
                   item.Patient.NomPatient.ToUpper().Contains(searchTerm) ||
                   item.Patient.PrenomPatient.ToUpper().Contains(searchTerm) ||
                    item.consultation.MotifConsult.ToUpper().Contains(searchTerm)
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

        //add new consultation
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Select_patient_control selectpatient = new Select_patient_control(user);
                Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                if (acceuil != null)
                {
                    acceuil.ChangerControleur(selectpatient);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }      
        }

        //imprimer ordonnance
        private void btn_imprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Consult.SelectedItem != null)
                {
                    GestionConsultation_Class consult = (GestionConsultation_Class)datagrid_Consult.SelectedItem;
                    new Imprimer_Ordonnnace(consult).ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //edit consultation
        private void EditConsult_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Consult.SelectedItem != null)
                {
                    GestionConsultation_Class consult = (GestionConsultation_Class)datagrid_Consult.SelectedItem;

                    Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                    if (acceuil != null)
                    {
                        acceuil.ChangerControleur(new Details_Consultation(consult.consultation.IdConsult, user));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //new consultation btn datagrid
        private void New_consult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Consult.SelectedItem != null)
                {
                    GestionConsultation_Class consult = (GestionConsultation_Class)datagrid_Consult.SelectedItem;
                    AddConsultation_Controle addconsult =
                       new AddConsultation_Controle(Convert.ToInt32(consult.Patient.IDPatient), user);
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
