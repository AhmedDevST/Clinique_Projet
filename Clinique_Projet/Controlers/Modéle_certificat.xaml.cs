using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Modéle_certificat.xaml
    /// </summary>
    public partial class Modéle_certificat : UserControl
    {
        Utilisateur_Class user;
        int yes { get; set; }
        public Modéle_certificat(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                this.yes = 0;
                date.Text = DateTime.Now.ToShortDateString();
                apte.IsChecked = true;
                Nom_Docteur.Text = " بوكري حسناء ";
                grid.Children.Add(new maladie(1,user));

            }catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N M099 !!");
            }      
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        //---------- Certificate Medical : arrete de travail ---------------

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
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("Les Données sont Incohérentes!!");
            }catch(Exception)
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
            catch(Exception)
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
            catch(Exception)
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
            catch(Exception)
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

        //  Data grid 
        private void check_medical_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grid != null)
                {
                    grid.Children.Clear();
                    grid.Children.Add(new maladie(1, user));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // --------Certificate Medical : PROlongation de travail ------------

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

                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("Les Données sont Incohérentes!!");
            }
            catch(Exception)
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
            catch(Exception)
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
            catch(Exception)
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
            catch(Exception)
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
        
        //------------ Certificate de mariage ----------------

        // btn imprimer
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (nom_patient_mariage.Text != "" || garant.Text != "" || cin.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous Enregistrer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //ajouter dans la base de donnnee
                        certificat add_certificate = new certificat(nom_patient_mariage.Text, garant.Text, cin.Text, DateTime.Now, description.Text);
                        if (add_certificate.ajouter_mariage())
                        {
                            new succes().ShowDialog();

                            //print
                            PrintDialog dialog = new PrintDialog();
                            if (dialog.ShowDialog() == true)
                            {
                                dialog.PrintVisual(print_certifcat_maraige, "شهادة البلوغ للزواج");
                                vider_champs_certificate_mariage();
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
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                vider_champs_certificate_mariage();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //  Data grid 
        private void check_mariage_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                grid.Children.Clear();
                grid.Children.Add(new maladie(3, user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //-----------Certificate aptitude physique -------------

        // btn imprimer
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CIN_patient_apptitude_phy.Text == "" || Nom_patient_Apptitude_phy.Text == "" || apte.IsChecked == true && inapte.IsChecked == true || apte.IsChecked == false && inapte.IsChecked == false)

                    MessageBox.Show("Les Données sont Incohérentes!!");
                else
                {
                    //les donnee sont valides
                    MessageBoxResult result = MessageBox.Show("Voulez Vous Enregistrer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        certificat add_cert;
                        if (apte.IsChecked == true)
                        {
                            add_cert = new certificat(Nom_patient_Apptitude_phy.Text, DateTime.Now, CIN_patient_apptitude_phy.Text, "APTE", remarque_supp.Text);
                        }
                        else
                            add_cert = new certificat(Nom_patient_Apptitude_phy.Text, DateTime.Now, CIN_patient_apptitude_phy.Text, "INAPTE", remarque_supp.Text);

                        //ajouter dans la base de donnee
                        if (add_cert.ajouter_apte())
                        {
                            new succes().ShowDialog();
                            //print
                            PrintDialog dialog = new PrintDialog();
                            if (dialog.ShowDialog() == true)
                            {
                                dialog.PrintVisual(print_certificate_aptitude_Physique, "CERTIFICAT D'APTITUDE PHYSIQUE");
                                vider_champs_aptitude_phy();
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
            }catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // btn annuler
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                vider_champs_aptitude_phy();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //  Data grid 
        private void check_aptitude_physique_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                grid.Children.Clear();
                grid.Children.Add(new maladie(2, user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // --------------- Certificate aptitude mariage -----------------

        // btn imprimer
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                date_mariage_act1.SelectedDate = DateTime.Now;
                if (patient_apt_mariage.Text != "" && date_mariage_act1.SelectedDate !=null && cin_apt_mariage.Text != "")
                {
                    //les donnes sont valides
                    MessageBoxResult result = MessageBox.Show("Voulez Vous Enregistrer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        date_mariage_act.Visibility=Visibility.Visible;
                        date_mariage_act1.Visibility=Visibility.Collapsed;
                        date_mariage_act.Text = date_mariage_act1.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        certificat add_cert = new certificat(patient_apt_mariage.Text, "0", cin_apt_mariage.Text, Convert.ToDateTime(date_mariage_act.Text), description_apt_mariage.Text);
                        if (add_cert.ajouter_mariage())
                        {
                            new succes().ShowDialog();
                            //print
                            PrintDialog dialog = new PrintDialog();
                            if (dialog.ShowDialog() == true)
                            {
                                dialog.PrintVisual(print_certificat_act_mariage, " إبرام عقد الزواج");
                                vider_champs_mariage_act();
                            }
                        }
                        else MessageBox.Show("Opération d'entrée innatendu !!");
                    }
                }
                else MessageBox.Show("Les Données sont Incohérentes!!");
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // btn annuler
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                vider_champs_mariage_act();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //  Data grid 
        private void check_aptitude_mariage_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                grid.Children.Clear();
                grid.Children.Add(new maladie(4, user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void vider_textbox(TextBox T)
        {
            T.Text = "";
            T.TextAlignment = TextAlignment.Center;
        }
        private void vider_textBoxes(TextBox textBox, TextBox textBox1, DatePicker datePicker, DatePicker datePicker1)
        {
            textBox.Text = "";
            textBox1.Text = "0";

            datePicker.SelectedDate = null;
            datePicker1.SelectedDate = null;
        }
        //calculer nombre de jours
        private double nb_jour(DatePicker D1, DatePicker D2)
        {
            TimeSpan v = Convert.ToDateTime(D1.SelectedDate) - Convert.ToDateTime(D2.SelectedDate);
            if (v.TotalDays >= 0)
                return v.TotalDays + 1;
            return 0;
        }
        private void vider_champs_certificate_mariage()
        {
            nom_patient_mariage.Text = "";
            garant.Text = "";
            cin.Text = "";
            description.Text = "";
        }
        private void vider_champs_aptitude_phy()
        {
            Nom_patient_Apptitude_phy.Text = "";
            CIN_patient_apptitude_phy.Text = "";
            remarque_supp.Text = "";
            apte.IsChecked = true;
            inapte.IsChecked = false;
        }
        private void vider_champs_mariage_act()
        {
            patient_apt_mariage.Text = "";
            cin_apt_mariage.Text = "";
            description_apt_mariage.Text = "";
            date_mariage_act.Text = "";
        }

        //-----------------------  END  METHODES    ------------------------------------ ----

    }
}
