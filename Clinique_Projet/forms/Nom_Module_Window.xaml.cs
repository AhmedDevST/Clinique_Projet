using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Nom_Module_Window.xaml
    /// </summary>
    public partial class Nom_Module_Window : Window
    {
        public string name_module { get; set; }
        public Nom_Module_Window()
        {
            try
            {
                InitializeComponent();
                name_module = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        // -------------------------- SATRT EVENTS :  -------------------------------

        private void image_close_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Nom_module_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Valide_Nom_Module();
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }


        private void Save_Namemodule_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Valide_Nom_Module())
                {
                    name_module = Nom_module_textbox.Text;
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        // -------------------------- END EVENTS :  -------------------------------


        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        private bool Valide_Nom_Module()
        {
            if (Nom_module_textbox.Text.Length == 0)
            {
                Remarque_nom.Text = "le champs est non valide";
                Remarque_nom.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                Remarque_nom.Text = "le champs est valide";
                Remarque_nom.Foreground = Brushes.Green;
                return true;
            }
        }


        //-----------------------  END  METHODES  :  ------------------------------------ ----

       
    }
}
