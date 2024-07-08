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
    /// Logique d'interaction pour imprimer_analyses.xaml
    /// </summary>
    public partial class imprimer_analyses : Window
    {
        ObservableCollection<AnalyseClass> list_analyse_print = new ObservableCollection<AnalyseClass>();
        public int id_consult;
        public imprimer_analyses(int id_consult, int id_patient)
        {
            try
            {
                InitializeComponent();
                this.id_consult = id_consult;
                datagrid_analyse.ItemsSource = GestionBilan_class.Display_analyse_consult(id_consult);
                list_analyse_print = GestionBilan_class.Display_analyse_consult(id_consult);
                Date_Analyses.Text = DateTime.Now.ToShortDateString();
                patientName.Text = PatientClass.SelectNomPatient(id_patient);
                Load_prochaine_rdv(id_consult);
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
                datagrid_analyse.ItemsSource = list_analyse_print;
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() == true)
                {
                    dialog.PrintVisual(Analyses, "Analyses");
                    Close();
                }
                Column_cheked.Visibility = Visibility.Visible;
                datagrid_analyse.ItemsSource = GestionBilan_class.Display_analyse_consult(id_consult);
                list_analyse_print = GestionBilan_class.Display_analyse_consult(id_consult);
            }
            catch (Exception)
            {
                Close();
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //check analyse 
        private void chek_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_analyse.SelectedItem != null)
                {
                    AnalyseClass analyse = (AnalyseClass)datagrid_analyse.SelectedItem;
                    list_analyse_print.Add(analyse);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //uncheck analyse 
        private void chek_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_analyse.SelectedItem != null)
                {
                    AnalyseClass analye = (AnalyseClass)datagrid_analyse.SelectedItem;
                    var item = list_analyse_print.FirstOrDefault(b => b.IdAnalyse == analye.IdAnalyse);
                    list_analyse_print.Remove(item);
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
