using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Notification_Control.xaml
    /// </summary>
    public partial class Notification_Control : UserControl
    {
        public Utilisateur_Class user;
        ObservableCollection<Notification_class> notifications = new ObservableCollection<Notification_class>();

        public Notification_Control(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                datagrid_notification.ItemsSource = Notification_class.Display_all_notification();
                notifications = Notification_class.Display_all_notification(); 
                load_combobx_type_operation();
                load_combobx_date();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N A203 !!");
            }   
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        //choisir le type de notifications
        private void combobox_Type_Operation_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_Type_Operation.SelectedValue != null)
                {
                    combobox_date.SelectedIndex = 0;
                    int value = Convert.ToInt32(combobox_Type_Operation.SelectedValue);
                    switch (value)
                    {
                        //tous
                        case 1:
                            datagrid_notification.ItemsSource = Notification_class.Display_all_notification();
                            notifications = Notification_class.Display_all_notification();
                            break;
                        //patient
                        case 2:
                            datagrid_notification.ItemsSource = Notification_class.Display_notification_by_Categorie("pat");
                            notifications = Notification_class.Display_notification_by_Categorie("pat");
                            break;
                        //consultation
                        case 3:
                            datagrid_notification.ItemsSource = Notification_class.Display_notification_by_Categorie("cons");
                            notifications = Notification_class.Display_notification_by_Categorie("cons");
                            break;
                        //rendez vous
                        case 4:
                            datagrid_notification.ItemsSource = Notification_class.Display_notification_by_Categorie("rdv");
                            notifications = Notification_class.Display_notification_by_Categorie("rdv");
                            break;
                        //tous
                        default:
                            datagrid_notification.ItemsSource = Notification_class.Display_all_notification();
                            notifications = Notification_class.Display_all_notification();
                            combobox_Type_Operation.SelectedIndex = 0;
                            break;
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N A302 !!");
            }
        }

        //show operation
        private void show_operation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_notification.SelectedItem != null)
                {
                    Notification_class notification = (Notification_class)datagrid_notification.SelectedItem;
                    switch (notification.Type_operation)
                    {
                        case "pat":
                            Fiche_Patient_Controlle patient_Controlle =
                               new Fiche_Patient_Controlle(notification.Id_operation, user);
                            Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                            if (acceuil != null)
                            {
                                acceuil.ChangerControleur(patient_Controlle);
                            }
                            break;
                        case "cons":
                            Acceuil_Docteur acceuil1 = Window.GetWindow(this) as Acceuil_Docteur;
                            if (acceuil1 != null)
                            {
                                acceuil1.ChangerControleur(new Details_Consultation(notification.Id_operation, user));
                            }
                            break;

                        case "rdv":
                            detail_rendezVous detail_Rendez = new detail_rendezVous(notification.Id_operation, user);
                            detail_Rendez.ShowDialog();
                            //si on click pour voir la  fiche patient:
                            if (detail_Rendez.IsLoad_fiche_patient)
                            {
                                Acceuil_Docteur acceuil_rdv = Window.GetWindow(this) as Acceuil_Docteur;
                                if (acceuil_rdv != null)
                                {
                                    acceuil_rdv.ChangerControleur(new Fiche_Patient_Controlle(detail_Rendez.Patient_ID, user));
                                }
                            }
                            combobox_Type_Operation.SelectedIndex = 3;
                            datagrid_notification.ItemsSource = Notification_class.Display_notification_by_Categorie("rdv");
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N A206 !!");
            }
        }

        //choisir la duree  de notifications
        private void combobox_date_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combobox_date.SelectedValue != null)
                {
                    int value = Convert.ToInt32(combobox_date.SelectedValue);
                    switch (value)
                    {
                        //tous
                        case 1:
                            datagrid_notification.ItemsSource = Notification_class.Display_all_notification();
                            notifications = Notification_class.Display_all_notification();
                            combobox_Type_Operation.SelectedIndex = 0;
                            break;
                        //ajourdui
                        case 2:
                            ObservableCollection<Notification_class> notifications_today =
                                new ObservableCollection<Notification_class>(notifications.Where(item => item.Date_notification >= DateTime.Today && item.Date_notification <= DateTime.Now));

                            datagrid_notification.ItemsSource = notifications_today;
                            break;
                        //ce semaine
                        case 3:
                            ObservableCollection<Notification_class> notifications_7 =
                                new ObservableCollection<Notification_class>(notifications.Where(item => item.Date_notification <= DateTime.Today && item.Date_notification >= DateTime.Now.AddDays(-7)));

                            datagrid_notification.ItemsSource = notifications_7;
                            break;
                        //ce mois
                        case 4:
                            ObservableCollection<Notification_class> notifications_mois =
                                new ObservableCollection<Notification_class>(notifications.Where(item => item.Date_notification >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) && item.Date_notification <= DateTime.Now));

                            datagrid_notification.ItemsSource = notifications_mois;
                            break;
                        // tous
                        default:
                            datagrid_notification.ItemsSource = Notification_class.Display_all_notification();
                            notifications = Notification_class.Display_all_notification();
                            combobox_Type_Operation.SelectedIndex = 0;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu N A208 !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void load_combobx_type_operation()
        {

            List<KeyValuePair<int, string>> List_cat = new List<KeyValuePair<int, string>>();
            List_cat.Add(new KeyValuePair<int, string>(1, "Tous"));
            List_cat.Add(new KeyValuePair<int, string>(2, "Patient"));
            List_cat.Add(new KeyValuePair<int, string>(3, "Consultation"));
            List_cat.Add(new KeyValuePair<int, string>(4, "Rendez Vous"));
            combobox_Type_Operation.ItemsSource = List_cat;
            combobox_Type_Operation.DisplayMemberPath = "Value";
            combobox_Type_Operation.SelectedValuePath = "Key";
            combobox_Type_Operation.SelectedIndex = 0;
        }
        private void load_combobx_date()
        {

            List<KeyValuePair<int, string>> List_cat = new List<KeyValuePair<int, string>>();
            List_cat.Add(new KeyValuePair<int, string>(1, "Tous"));
            List_cat.Add(new KeyValuePair<int, string>(2, "Aujourd'hui"));
            List_cat.Add(new KeyValuePair<int, string>(3, "Cette semaine"));
            List_cat.Add(new KeyValuePair<int, string>(4, "Ce mois "));
            combobox_date.ItemsSource = List_cat;
            combobox_date.DisplayMemberPath = "Value";
            combobox_date.SelectedValuePath = "Key";
            combobox_date.SelectedIndex = 0;
        }

        //-----------------------  END  METHODES    ------------------------------------ ----
    }
}
