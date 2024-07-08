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
    /// Logique d'interaction pour maladie.xaml
    /// </summary>
    public partial class maladie : UserControl
    {
        public int a { get; set; }
        ObservableCollection<certificat> c;
        Utilisateur_Class user;
        public maladie(int a,Utilisateur_Class user)
        {
            try 
            { 
                InitializeComponent();
                this.a = a;
                this.user = user;
                droit_user();
                loadDataGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- SATRT EVENTS :  -------------------------------
        // supprimer un certificat apt_physique
        private void supprimer_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if (datagrid2.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous supprimer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        certificat p = (certificat)datagrid2.SelectedItem;
                    
                        certificat.Delete_certificat(p.id,"apt_physique");
                        new succes().ShowDialog();
                        loadDataGrid();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //supprimer un certificate medical
        private void supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if (datagrid1.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous supprimer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        certificat p = (certificat)datagrid1.SelectedItem;
                        certificat.Delete_certificat(p.id, "certificat");
                        new succes().ShowDialog();
                        loadDataGrid();

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //supprimer un certificate mariage
        private void supprimer_Click_2(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (datagrid3.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous supprimer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        certificat p = (certificat)datagrid3.SelectedItem;
                        certificat.Delete_certificat(p.id, "mariage");
                        new succes().ShowDialog();
                        loadDataGrid();

                    }
                }
                if (datagrid4.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez Vous supprimer cette certificat!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        certificat p = (certificat)datagrid4.SelectedItem;
                        certificat.Delete_certificat(p.id, "mariage");
                        new succes().ShowDialog();
                        loadDataGrid();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        
        //recherche
        private void mytext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                search.BorderBrush = Brushes.Blue;
                string searchTerm = mytext.Text;   //recupere la valeur de textbox
                if (!string.IsNullOrEmpty(searchTerm)) // si le textbox n'est pas vide
                {
                    var filteredData = c.Where(item =>

                    item.name.ToLower().Contains(searchTerm.ToLower()) ||
                    item.date_it.ToString().Contains(searchTerm));

                    datagrid1.ItemsSource = filteredData;
                    datagrid2.ItemsSource = filteredData;
                    datagrid3.ItemsSource = filteredData;
                    datagrid4.ItemsSource = filteredData;//affecter le result a datagrid
                }
                else
                {
                    datagrid1.ItemsSource = c; //si le textbox est vide on loaddatagrid
                    datagrid2.ItemsSource = c; //si le textbox est vide on loaddatagrid
                    datagrid3.ItemsSource = c; //si le textbox est vide on loaddatagrid
                    datagrid4.ItemsSource = c; //si le textbox est vide on loaddatagrid
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu  !!");
            }
        }

        // -------------------------- END EVENTS :  -------------------------------

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----
        private void droit_user()
        {
            if (Convert.ToInt32(user.Role_user) == 2)
            {
                delete_btn_Cmedical.Visibility = Visibility.Collapsed;
                delete_btn_CapctM.Visibility = Visibility.Collapsed;
                delete_btn_Cmriage.Visibility = Visibility.Collapsed;
                delete_btn_Cphysique.Visibility = Visibility.Collapsed;
            }
        }
        private void loadDataGrid()
        {
            switch (a)
            {
                case 1:
                    datagrid1.ItemsSource = certificat.display_certificat();
                    datagrid2.Visibility = Visibility.Hidden;
                    datagrid3.Visibility = Visibility.Hidden;
                    datagrid4.Visibility = Visibility.Hidden;
                    datagrid1.Visibility = Visibility.Visible;
                    c = certificat.display_certificat();
                    break;

                //apt physique
                case 2:
                    datagrid2.Visibility = Visibility.Visible;
                    datagrid1.Visibility = Visibility.Hidden;
                    datagrid3.Visibility = Visibility.Hidden;
                    datagrid4.Visibility = Visibility.Hidden;
                    datagrid2.ItemsSource = certificat.display();
                    c = certificat.display();
                    break;

                case 3:
                    datagrid1.Visibility = Visibility.Hidden;
                    datagrid2.Visibility = Visibility.Hidden;
                    datagrid3.Visibility = Visibility.Visible;
                    datagrid4.Visibility = Visibility.Hidden;
                    datagrid3.ItemsSource = certificat.display_certificat_mariage(0);
                    c = certificat.display_certificat_mariage(0);
                    break;

                case 4:
                    datagrid1.Visibility = Visibility.Hidden;
                    datagrid2.Visibility = Visibility.Hidden;
                    datagrid3.Visibility = Visibility.Hidden;
                    datagrid4.Visibility = Visibility.Visible;
                    datagrid4.ItemsSource = certificat.display_certificat_mariage(1);
                    c = certificat.display_certificat_mariage(0);
                    break;
            }

        }
      
        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
