using Clinique_Projet.Modal;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Certificate_medical_window.xaml
    /// </summary>
    public partial class Certificate_medical_window : Window
    {
        public Certificate_medical_window(int id_patient)
        {
            try
            {
                InitializeComponent();
                Nom_patient.Text = PatientClass.SelectNomPatient(id_patient);
                Nom_patient1.Text = PatientClass.SelectNomPatient(id_patient);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }
        // -------------------------- SATRT EVENTS :  -------------------------------

        //---------Certificate Medical : arrete de travail --------------

        // imprimer btn
        private void Btn_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // validation de donnes
                if (NbJours_ArretTrav.Text != "0" && Nom_patient.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous Enregistrer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //ajouter dans la base de donnee
                        certificat certificat_line = new certificat(Nom_patient.Text.ToString(), Convert.ToInt32(NbJours_ArretTrav.Text),
                              DateDebut_ArretTrav.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), DateFin_ArretTrav.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.Now, "arrete");
                        //si ajouter est effectue avec sucess
                        if (certificat_line.add_certificat())
                        {
                            new succes().ShowDialog();
                            //print
                            PrintDialog dialog = new PrintDialog();
                            if (dialog.ShowDialog() == true)
                            {
                                DateDebut_ArretTrav.Visibility = Visibility.Collapsed;
                                DateFin_ArretTrav.Visibility = Visibility.Collapsed;
                                DateDebut_ArretTrav_text.Text = DateDebut_ArretTrav.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateDebut_ArretTrav_text.Visibility = Visibility.Visible;
                                DateFin_ArretTrav_text.Text = DateFin_ArretTrav.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateFin_ArretTrav_text.Visibility = Visibility.Visible;

                                dialog.PrintVisual(Print_Certificate, "Certifiact Medical");

                                vider_textBoxes(Nom_patient, NbJours_ArretTrav, DateDebut_ArretTrav, DateFin_ArretTrav);
                                DateDebut_ArretTrav_text.Visibility = Visibility.Collapsed;
                                DateFin_ArretTrav_text.Visibility = Visibility.Collapsed;
                                DateDebut_ArretTrav.Visibility = Visibility.Visible;
                                DateFin_ArretTrav.Visibility = Visibility.Visible;
                                this.Close();
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("Les Données sont Incohérentes!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler btn
        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vider_textBoxes(Nom_patient, NbJours_ArretTrav, DateDebut_ArretTrav, DateFin_ArretTrav);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //event date debut arrete de tarvail
        private void DateDebut_ArretTrav_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateDebut_ArretTrav.SelectedDate != null && DateFin_ArretTrav.SelectedDate != null)
                {
                    NbJours_ArretTrav.Text = nb_jour(DateFin_ArretTrav, DateDebut_ArretTrav).ToString();
                }
                else NbJours_ArretTrav.Text = "0";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //event date fin arrete de tarvail
        private void DateFin_ArretTrav_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateDebut_ArretTrav.SelectedDate != null && DateFin_ArretTrav.SelectedDate != null)
                {
                    NbJours_ArretTrav.Text = nb_jour(DateFin_ArretTrav, DateDebut_ArretTrav).ToString();
                }
                else NbJours_ArretTrav.Text = "0";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Nom_patient_TouchUp(object sender, TouchEventArgs e)
        {
            try
            {
                vider_textbox(Nom_patient);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        //---------Certificate Medical : PROlongation de travail --------------

        // btn imprimer
        private void Btn_Print1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // validation de donnes
                if (NbrJours_Prolong1.Text != "0" && Nom_patient1.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous Enregistrer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        // //ajouter dans la base de donnee
                        certificat certificat_line = new certificat(Nom_patient1.Text.ToString(), Convert.ToInt32(NbrJours_Prolong1.Text),
                                DateDebut_Prolong1.SelectedDate.ToString(), DateFin_Prolong1.SelectedDate.ToString(), DateTime.Now, "Prolongation");
                        //si ajouter est effectue avec sucess
                        if (certificat_line.add_certificat())
                        {
                            new succes().ShowDialog();
                            //print
                            PrintDialog dialog = new PrintDialog();
                            if (dialog.ShowDialog() == true)
                            {
                                DateDebut_Prolong1.Visibility = Visibility.Collapsed;
                                DateFin_Prolong1.Visibility = Visibility.Collapsed;
                                DateDebut_Prolong1_text.Text = DateDebut_Prolong1.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateDebut_Prolong1_text.Visibility = Visibility.Visible;
                                DateFin_Prolong1_text.Text = DateFin_Prolong1.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateFin_Prolong1_text.Visibility = Visibility.Visible;

                                dialog.PrintVisual(Print_Certificate1, "Certifiact Medical");

                                vider_textBoxes(Nom_patient1, NbrJours_Prolong1, DateDebut_Prolong1, DateFin_Prolong1);
                                DateDebut_Prolong1_text.Visibility = Visibility.Collapsed;
                                DateFin_Prolong1_text.Visibility = Visibility.Collapsed;
                                DateDebut_Prolong1.Visibility = Visibility.Visible;
                                DateFin_Prolong1.Visibility = Visibility.Visible;
                                this.Close();
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("Les Données sont Incohérentes!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //btn annuler
        private void Btn_Annuler1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vider_textBoxes(Nom_patient1, NbrJours_Prolong1, DateDebut_Prolong1, DateFin_Prolong1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //event date fin 
        private void DateFin_Prolong1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateDebut_Prolong1.SelectedDate != null && DateFin_Prolong1.SelectedDate != null)
                {
                    NbrJours_Prolong1.Text = nb_jour(DateFin_Prolong1, DateDebut_Prolong1).ToString();
                }
                else NbrJours_Prolong1.Text = "0";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        //event date debut
        private void DateDebut_Prolong1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DateDebut_Prolong1.SelectedDate != null && DateFin_Prolong1.SelectedDate != null)
                {
                    NbrJours_Prolong1.Text = nb_jour(DateFin_Prolong1, DateDebut_Prolong1).ToString();
                }
                else NbrJours_Prolong1.Text = "0";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Nom_patient1_TouchUp(object sender, TouchEventArgs e)
        {
            try
            {
                vider_textbox(Nom_patient1);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        //calculer nombre de jours
        private double nb_jour(DatePicker D1, DatePicker D2)
        {
            TimeSpan v = Convert.ToDateTime(D1.SelectedDate) - Convert.ToDateTime(D2.SelectedDate);
            if (v.TotalDays >= 0)
                return v.TotalDays + 1;
            return 0;
        }
        private void vider_textBoxes(TextBox textBox, TextBox textBox1, DatePicker datePicker, DatePicker datePicker1)
        {
            textBox.Text = "";
            textBox1.Text = "0";

            datePicker.SelectedDate = null;
            datePicker1.SelectedDate = null;
        }
        private void vider_textbox(TextBox T)
        {
            T.Text = "";
            T.TextAlignment = TextAlignment.Center;
        }
        //-----------------------  END  METHODES    ------------------------------------ ----

    }
}
