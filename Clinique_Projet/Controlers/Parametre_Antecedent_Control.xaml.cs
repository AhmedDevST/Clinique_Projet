using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Prametre_Antecedent_Control.xaml
    /// </summary>
    public partial class Prametre_Antecedent_Control : UserControl
    {
        public Utilisateur_Class user;
        public TypeAntecedent Obj_TypeAntecdent;
        public Prametre_Antecedent_Control(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Load_TypeAntecedent();
                EditType_Anteced_btn.IsEnabled = false;
                Delete_TypeAnteced_btn.IsEnabled = false;
                this.user = user;
                Droits_user();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
       
        // -------------------------- SATRT EVENTS :  -------------------------------

        //show antecdents
        private void show_typeAnteced_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (datagrid_TypeAnteced.SelectedItem != null)
                {
                    Obj_TypeAntecdent = (TypeAntecedent)datagrid_TypeAnteced.SelectedItem;
                    Nom_TypeANteced.Text = Obj_TypeAntecdent.Nom_TypeANteced;
                    EditType_Anteced_btn.IsEnabled = true;
                    Delete_TypeAnteced_btn.IsEnabled = true;
                    Add_tyoeAnteced_btn.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // add antecdent
        private void Add_tyoeAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_TypeANteced.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Ajouter ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        TypeAntecedent TypeAntecedent = new TypeAntecedent(0, Nom_TypeANteced.Text);
                        if (TypeAntecedent.Add_TypeAntecdent())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            Load_TypeAntecedent();
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

        // update antecdents
        private void EditType_Anteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_TypeANteced.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifier ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        TypeAntecedent TypeAntecedent = new TypeAntecedent(Obj_TypeAntecdent.ID_TypeANteced, Nom_TypeANteced.Text);
                        if (TypeAntecedent.Update_TypeAntecdent())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs();
                            Load_TypeAntecedent();
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

        //delete antcedent
        private void Delete_TypeAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce type", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (TypeAntecedent.Delete_TypeAntecdent(Obj_TypeAntecdent.ID_TypeANteced))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs();
                        Load_TypeAntecedent();
                    }
                    else MessageBox.Show("impossible de supprimer ce type");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //annuler antecdent type
        private void Annuler_typeAnteced_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs();
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
                if (user.IsAdmin == 0) Delete_TypeAnteced_btn.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void Load_TypeAntecedent()
        {
            try
            {
                datagrid_TypeAnteced.ItemsSource = TypeAntecedent.DisplayTypeAnteced();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        private void initialiser_champs()
        {
            try
            {
                Nom_TypeANteced.Text = "";
                EditType_Anteced_btn.IsEnabled = false;
                Delete_TypeAnteced_btn.IsEnabled = false;
                Add_tyoeAnteced_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES   ------------------------------------ ----
    }
}
