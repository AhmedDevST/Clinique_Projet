using Clinique_Projet.Controlers;
using Clinique_Projet.Modal;
using System;
using System.Windows;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Parametres.xaml
    /// </summary>
    public partial class Parametres : Window
    {
        public Utilisateur_Class user;
        public Parametres(string parametre,Utilisateur_Class user)
        {
            try 
            {
                InitializeComponent();
                this.user = user;
                Initialiser_Control(parametre);
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        private void Initialiser_Control(string parametre)
        {
            try
            {
                Controle_Parametres.Children.Clear();
                switch (parametre)
                {
                    case "assurance": Controle_Parametres.Children.Add(new Parametre_Agent_Assurance(user)); break;
                    case "group_sang": Controle_Parametres.Children.Add(new Parametre_Group_Sang(user)); break;
                    case "antecedent": Controle_Parametres.Children.Add(new Prametre_Antecedent_Control(user)); break;
                    default: this.Close(); break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }     
        }
        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
