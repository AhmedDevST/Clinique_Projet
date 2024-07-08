using Clinique_Projet.connectionDb;
using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using System;
using System.Windows;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour DeatilsBilan.xaml
    /// </summary>
    public partial class DeatilsBilan : Window
    {
        GestionBilan_class gestion_Bilan;
        public SqlTableDependency<DMBilans> BilansSqlTableDependency;
        public DeatilsBilan(GestionBilan_class g)
        {
            try
            {
                InitializeComponent();
                gestion_Bilan = g;
                LoadData();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- SATRT EVENTS :  -------------------------------

        //open window 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Start_TableBilans_Dependency();
            }
            catch (Exception)
            {

            }
        }
      
        //close  window 
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Stop_TableBilans_Dependency();
            }
            catch (Exception)
            {

            }
        }
       
        //button fermer  window 
        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // -------------------------- END EVENTS :  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        private void LoadData()
        {
            TitreBilan.Text = "Bilan " + gestion_Bilan.Bilans.DateBilan.ToShortDateString();
            NomBilan_patient.Text = gestion_Bilan.Analyse.NomAnalyse;
            TypeBilan.Text = gestion_Bilan.TypeAnalyse.Nom_TypeBilan;
            Observation_Bilan.Text = gestion_Bilan.Bilans.Result_Analyse;
        }

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
            catch (Exception )
            {
            }
        }
        private void Changed_Bilans_Dependency(object sender, RecordChangedEventArgs<DMBilans> e)
        {
            try
            {
                DMBilans EntityBilans = e.Entity;
                if ((e.ChangeType != ChangeType.None) && (e.ChangeType != ChangeType.Insert))
                {
                    if (EntityBilans != null)
                    {
                        if (EntityBilans.Consult_id == gestion_Bilan.Bilans.ConsultID)
                        {
                            switch (e.ChangeType)
                            {
                                case ChangeType.Update:
                                    Dispatcher.Invoke(() =>
                                    {
                                        Observation_Bilan.Text = EntityBilans.Result_Analyse;
                                    });
                                    break;

                                case ChangeType.Delete:
                                    Dispatcher.Invoke(() =>
                                    {
                                        new Notification_window
                                                        (" cette bilan a ete  supprimer ")
                                                        .ShowDialog();
                                        this.Close();
                                    });
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
        
        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
