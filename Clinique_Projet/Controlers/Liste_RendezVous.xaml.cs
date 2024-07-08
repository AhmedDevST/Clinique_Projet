using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Liste_RendezVous.xaml
    /// </summary>
    public partial class Liste_RendezVous : UserControl
    {
        ObservableCollection<RendezVous> p;
       
        public Utilisateur_Class user { get; set; }

        public Liste_RendezVous(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user= user;
                p = RendezVous.DisplayRendezVous(-1);
                datagrid1.ItemsSource = p;
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
       //recherche
        private void mytext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                search.BorderBrush = Brushes.Blue;
                string searchTerm = mytext.Text;   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = p.Where(item =>
                  item.nom.ToString().Contains(searchTerm) ||
                   item.prenom.ToString().Contains(searchTerm) ||
                    item.date.ToString().Contains(searchTerm) ||
                     item.type.Contains(searchTerm)
               );
                    datagrid1.ItemsSource = filteredData;  //affecter le result a datagrid
                }
                else datagrid1.ItemsSource = p; //si le textbox est vide on loaddatagrid
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // ouvrir fenetre : detail_rendezVous 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid1.SelectedItem != null)
                {
                    RendezVous pr = (RendezVous)datagrid1.SelectedItem;
                    detail_rendezVous detail_RendezVous1 = new detail_rendezVous(pr.id, user);
                    detail_RendezVous1.ShowDialog();
                    //si on click  dans feentre detail_rendezVous pour voir la  fiche patient:
                    if (detail_RendezVous1.IsLoad_fiche_patient)
                    {
                        Acceuil_Docteur acceuil = Window.GetWindow(this) as Acceuil_Docteur;
                        if (acceuil != null)
                        {
                            acceuil.ChangerControleur(new Fiche_Patient_Controlle(detail_RendezVous1.Patient_ID, user));
                        }
                    }
                    else
                    {
                        p = RendezVous.DisplayRendezVous(-1);
                        datagrid1.ItemsSource = p;
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }

        }

        //supprimer le rendez vous 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                RendezVous pr = (RendezVous)datagrid1.SelectedItem;
                MessageBoxResult res = MessageBox.Show("VOUS VOULLEZ SUPPRIMER CE RENDEZ VOUS", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    RendezVous.supprimerRendezVous(pr.id);
                    new succes().ShowDialog();
                    p = RendezVous.DisplayRendezVous(-1);
                    datagrid1.ItemsSource = p;
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
