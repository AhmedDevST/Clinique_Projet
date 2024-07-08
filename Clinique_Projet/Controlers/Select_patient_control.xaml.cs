using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Select_patient_control.xaml
    /// </summary>
    public partial class Select_patient_control : UserControl
    {
        List<KeyValuePair<int, string>> ListPatient = PatientClass.ListPatient();
        public Utilisateur_Class user;
        public Select_patient_control(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                combobox_patient.ItemsSource = ListPatient;
                combobox_patient.DisplayMemberPath = "Value";
                combobox_patient.SelectedValuePath = "Key";
                this.user = user;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
           
        }
        // -------------------------- SATRT EVENTS :  -------------------------------

        // ouvrir le  controle de ajouter nouvelle consultation lorsque click sur le button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (combobox_patient.SelectedValue != null)
                {
                    AddConsultation_Controle addconsult =
                        new AddConsultation_Controle(Convert.ToInt32(combobox_patient.SelectedValue),user);
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

        //recherche in combobox
        private void combobox_patient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                string searchTerm = combobox_patient.Text;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var filteredItems = ListPatient.Where(item => item.Value.ToLower().Contains(searchTerm.ToLower())).ToList();

                    if (filteredItems.Count() > 0)
                    {
                        combobox_patient.ItemsSource = filteredItems;
                        combobox_patient.IsDropDownOpen = true;
                    }
                    else
                    {
                        combobox_patient.IsDropDownOpen = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }      
        }

        //click sur button entrer
        private void combobox_patient_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (combobox_patient.SelectedValue != null)
                    {
                        AddConsultation_Controle addconsult =
                            new AddConsultation_Controle(Convert.ToInt32(combobox_patient.SelectedValue), user);
                        Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                        if (acceuil != null)
                        {
                            acceuil.ChangerControleur(addconsult);

                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }    
        }

        // -------------------------- END EVENTS   -------------------------------

    }
}
