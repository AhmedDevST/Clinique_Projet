using Clinique_Projet.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Window
    {
        public Utilisateur_Class user;
        public Confirmation(Utilisateur_Class user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            //update status :deconnect
            Utilisateur_Class.Update_Status_User(user.id_user, 0);
            Application.Current.Shutdown();
        }
    }
}
