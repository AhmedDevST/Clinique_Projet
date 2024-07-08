using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Parametre_Agent_Assurance.xaml
    /// </summary>
    public partial class Parametre_Agent_Assurance : UserControl
    {
        public AssuranceClass Obj_Assurance;
        public Utilisateur_Class user;
        public Parametre_Agent_Assurance(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Load_Assurance();
                Edit_assurance_btn.IsEnabled = false;
                Delete_assurance_btn.IsEnabled = false;
                this.user = user;
                Droits_user();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }  
        }

        // -------------------------- SATRT EVENTS :  -------------------------------

        //button show assurance
        private void show_Assurance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_Assurance.SelectedItem != null)
                {
                    Obj_Assurance = (AssuranceClass)datagrid_Assurance.SelectedItem;
                    Nom_Assurance.Text = Obj_Assurance.NomAssurance;
                    Edit_assurance_btn.IsEnabled = true;
                    Delete_assurance_btn.IsEnabled = true;
                    Add_assurance_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button add
        private void Add_assurance_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_Assurance.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Ajouter cette agents", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        AssuranceClass assurance = new AssuranceClass(0, Nom_Assurance.Text);
                        if (assurance.Add_Assurance())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_assurance();
                            Load_Assurance();
                        }
                    }
                }
                else MessageBox.Show("le champs est vide!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button edit 
        private void Edit_assurance_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_Assurance.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifer cette agent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        AssuranceClass assurance = new AssuranceClass(Obj_Assurance.IdAssurance, Nom_Assurance.Text);
                        if (assurance.Update_Assurance())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_assurance();
                            Load_Assurance();
                        }
                    }
                }
                else MessageBox.Show("le champs est vide!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button delete
        private void Delete_assurance_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce agent", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (AssuranceClass.Delete_Assurance(Obj_Assurance.IdAssurance))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs_assurance();
                        Load_Assurance();
                    }
                    else MessageBox.Show("impossible de supprimer cette agent");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //button annuler
        private void Annuler_assurance_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs_assurance();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS   -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void Droits_user()
        {
            try
            {
                if (user.IsAdmin == 0) Delete_assurance_btn.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Load_Assurance()
        {
            try
            {
                datagrid_Assurance.ItemsSource = AssuranceClass.DisplayAssurance1();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void initialiser_champs_assurance()
        {
            try
            {
                Nom_Assurance.Text = "";
                Edit_assurance_btn.IsEnabled = false;
                Delete_assurance_btn.IsEnabled = false;
                Add_assurance_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES    ------------------------------------ ----
    }
}
