using Clinique_Projet.DatabaseModels;
using Clinique_Projet.Modal;
using System;
using System.Windows;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using Clinique_Projet.connectionDb;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Details_Ordonnance.xaml
    /// </summary>
    public partial class Details_Ordonnance : Window
    {
        GestionOrdonnance_Class obj_ordonnance;
        public SqlTableDependency<DMOrdonnance> OrdonnanceSqlTableDependency;
        public Details_Ordonnance(GestionOrdonnance_Class ordonnance)
        {
            try
            {
                InitializeComponent();
                obj_ordonnance = ordonnance;
                LoadData();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu ");
            }
        }
       
        
        // -------------------------- SATRT EVENTS :  -------------------------------
       
        // fermer fenetre       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Stop_TableOrdonnance_Dependency();
            }
            catch (Exception) { }
        }
        
        //ouvrir fenetre
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Start_TableOrdonnance_Dependency();
            }
            catch (Exception) { }
        }

        // button fermer fenetre
        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu ");
            }
        }

        // -------------------------- END EVENTS :  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void LoadData()
        {
            TitreOrdonnace.Text = "Ordonnance " + obj_ordonnance.ordonnance.DateOrdonnace.ToShortDateString();
            NomOrdonnanc.Text = obj_ordonnance.Medicament.NomMedcament;
            Typeordonnance.Text = obj_ordonnance.CatMedicament.Nom_CatMedicament;
            posologie.Text = obj_ordonnance.ordonnance.Posologie_Ordonnace;
            NoteOrdonnanc.Text = obj_ordonnance.ordonnance.Note_Plus;
            Qunatite.Text = obj_ordonnance.ordonnance.Quantite.ToString();
        }

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
                if ((e.ChangeType != ChangeType.None) && (e.ChangeType != ChangeType.Insert))
                {
                    if (EntityOrdonnance != null)
                    {
                        if (EntityOrdonnance.Consult_id == obj_ordonnance.ordonnance.Consult_Ordonnace)
                        {
                            switch (e.ChangeType)
                            {
                                case ChangeType.Update:
                                    Dispatcher.Invoke(() =>
                                    {
                                        posologie.Text = EntityOrdonnance.Posologie;
                                        NoteOrdonnanc.Text = EntityOrdonnance.Note_Supplimentaire;
                                        Qunatite.Text = EntityOrdonnance.Quantite.ToString();
                                    });
                                    break;

                                case ChangeType.Delete:
                                    Dispatcher.Invoke(() =>
                                    {
                                        new Notification_window
                                                        (" cette Ordonnonce a ete  supprimer par un autre utilisateur")
                                                        .ShowDialog();
                                        this.Close();
                                    });
                                    break;
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

        //-----------------------  END  METHODES  :  ------------------------------------ ----      
    }
}
