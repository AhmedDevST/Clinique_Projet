using System;
using System.Windows;
using System.Windows.Input;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Notification_window.xaml
    /// </summary>
    public partial class Notification_window : Window
    {
        public Notification_window(string  notification)
        {
            try
            {
                InitializeComponent();
                text_notification.Text = notification;
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- SATRT EVENTS :  -------------------------------
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
        // -------------------------- END EVENTS :  -------------------------------

    }
}
