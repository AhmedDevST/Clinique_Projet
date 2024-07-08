using Clinique_Projet.forms;
using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour Acount_Settigns_Control.xaml
    /// </summary>
    public partial class Acount_Settigns_Control : UserControl
    {
        public Utilisateur_Class user;
        public Acount_Settigns_Control(Utilisateur_Class user)
        {
            try
            { 
                InitializeComponent();
                data_grid_user.ItemsSource = Utilisateur_Class.DisplayUser_Datagrid();
                this.user = user;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- SATRT EVENTS :  -------------------------------
       
        //nouveau utilisateur:window 
        private void btn_add_user_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                new Nouveau_User().ShowDialog();
                //load data utilisateur
                data_grid_user.ItemsSource = Utilisateur_Class.DisplayUser_Datagrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // info  utilisateur:window 
        private void details_user_Click_1(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (data_grid_user.SelectedItem != null)
                {
                    Utilisateur_Class user = (Utilisateur_Class)data_grid_user.SelectedItem;
                    new edit_info_user(user.id_user).ShowDialog();
                    //load data utilisateur
                    data_grid_user.ItemsSource = Utilisateur_Class.DisplayUser_Datagrid();
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
