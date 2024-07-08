using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;
namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Parametre_Group_Sang.xaml
    /// </summary>
    public partial class Parametre_Group_Sang : UserControl
    {
        public GroupSangClass Obj_GroupSang;
        public Utilisateur_Class user;
        public Parametre_Group_Sang(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Load_GroupSang();
                Edit_groupsang_btn.IsEnabled = false;
                Delete_Groupsang_btn.IsEnabled = false;
                this.user = user;
                Droits_user();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
     
        //annuler 
        private void Annuler_groupsang_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initialiser_champs_groupsang();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //modifer
        private void Edit_groupsang_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_GroupSang.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez Modifer ce group de sang", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        GroupSangClass sangClass = new GroupSangClass(Obj_GroupSang.IdGroupSang, Nom_GroupSang.Text);
                        if (sangClass.Update_GroupSang())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_groupsang();
                            Load_GroupSang();
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

        //supprimer
        private void Delete_Groupsang_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("vous voulllez supprimer ce group de sang", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (GroupSangClass.Delete_GroupSang(Obj_GroupSang.IdGroupSang))
                    {
                        MessageBox.Show("l'opeartion est bien enregistrer");
                        initialiser_champs_groupsang();
                        Load_GroupSang();
                    }
                    else MessageBox.Show("impossible de supprimer ce group");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //ajouter
        private void Add_GroupSang_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Nom_GroupSang.Text))
                {
                    MessageBoxResult res = MessageBox.Show("vous voulllez ajouter ce group de sang", "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        GroupSangClass sangClass = new GroupSangClass(0, Nom_GroupSang.Text);
                        if (sangClass.Add_GroupSang())
                        {
                            MessageBox.Show("les donnes bien enregistrer");
                            initialiser_champs_groupsang();
                            Load_GroupSang();
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

        //afficher
        private void show_groupsang_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagrid_GroupSang.SelectedItem != null)
                {
                    Obj_GroupSang = (GroupSangClass)datagrid_GroupSang.SelectedItem;
                    Nom_GroupSang.Text = Obj_GroupSang.NonmGroupSang;
                    Edit_groupsang_btn.IsEnabled = true;
                    Delete_Groupsang_btn.IsEnabled = true;
                    Add_GroupSang_btn.IsEnabled = false;
                }
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
                if (user.IsAdmin == 0) Delete_Groupsang_btn.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void initialiser_champs_groupsang()
        {
            try
            {
                Nom_GroupSang.Text = "";
                Edit_groupsang_btn.IsEnabled = false;
                Delete_Groupsang_btn.IsEnabled = false;
                Add_GroupSang_btn.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        private void Load_GroupSang()
        {
            try
            {
                datagrid_GroupSang.ItemsSource = GroupSangClass.Display_GroupSang();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES    ------------------------------------ ----
    }
}
