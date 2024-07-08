using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Imprimer_Ordonnnace.xaml
    /// </summary>
    public partial class Imprimer_Ordonnnace : Window
    {

        ObservableCollection<GestionOrdonnance_Class> list_ordonnonce_print = new ObservableCollection<GestionOrdonnance_Class>();
        public int id_consult;
        
        //first constructor
        public Imprimer_Ordonnnace(GestionConsultation_Class gestionConsultation_Class)
        {
            try
            {
                InitializeComponent();
                this.id_consult = gestionConsultation_Class.consultation.IdConsult;
                datagrid_ordonnonce.ItemsSource = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(gestionConsultation_Class.consultation.IdConsult);
                list_ordonnonce_print = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(gestionConsultation_Class.consultation.IdConsult); ;
                Date_Ordonnace.Text = DateTime.Now.ToShortDateString();
                patientName.Text = gestionConsultation_Class.Patient.NomPatient + " " + gestionConsultation_Class.Patient.PrenomPatient;
                Load_prochaine_rdv(gestionConsultation_Class.consultation.IdConsult);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //second constructor
        public Imprimer_Ordonnnace(int idConsult, int IdPatient)
        {
            try
            {
                InitializeComponent();
                id_consult = idConsult;
                datagrid_ordonnonce.ItemsSource = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(idConsult);
                list_ordonnonce_print = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(idConsult);
                patientName.Text = PatientClass.SelectNomPatient(IdPatient);
                Date_Ordonnace.Text = DateTime.Now.ToShortDateString();
                Load_prochaine_rdv(idConsult);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
       
        //annuler btn
        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //print btn
        private void Btn_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Column_cheked.Visibility = Visibility.Collapsed;
                datagrid_ordonnonce.ItemsSource = list_ordonnonce_print;
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() == true)
                {
                    dialog.PrintVisual(Ordonnnace, "Ordonnonce");
                    Close();
                }
                Column_cheked.Visibility = Visibility.Visible;
                datagrid_ordonnonce.ItemsSource = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(id_consult);
                list_ordonnonce_print = GestionOrdonnance_Class.DisplayOrdonnance_Datagrid(id_consult);
            }
            catch (Exception)
            {
                Close();
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // check ordonnonce
        private void chek_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_ordonnonce.SelectedItem != null)
                {
                    GestionOrdonnance_Class ordonnce = (GestionOrdonnance_Class)datagrid_ordonnonce.SelectedItem;
                    list_ordonnonce_print.Add(ordonnce);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        // uncheck ordonnonce
        private void chek_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_ordonnonce.SelectedItem != null)
                {
                    GestionOrdonnance_Class ordonnce = (GestionOrdonnance_Class)datagrid_ordonnonce.SelectedItem;
                    var item = list_ordonnonce_print.FirstOrDefault(b => b.Medicament.IdMedcament == ordonnce.Medicament.IdMedcament);
                    list_ordonnonce_print.Remove(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        // -------------------------- END EVENTS :  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void Load_prochaine_rdv(int id_consult)
        {
            ObservableCollection<RendezVous> list = Prochaine_RDV_class.Display_PRDV(id_consult);
            if (list.Count > 0)
            {
                ObservableCollection<RendezVous> list2 = new ObservableCollection<RendezVous>();
                //selection les date supereure date now
                foreach (var item in list)
                {
                    if (item.date >= DateTime.Now) list2.Add(item);
                }
                // minum date 
                if (list2.Count > 0)
                {
                    prochaine_rdv.Text = list2.Min(rdv => rdv.date).ToLongDateString();
                    prochaine_rdv_ar.Text = list2.Min(rdv => rdv.date).ToString("dddd d MMMM yyyy", new CultureInfo("ar-SA"));
                }
            }
        }

        //-----------------------  END  METHODES  :  ------------------------------------ ----

    }

}
