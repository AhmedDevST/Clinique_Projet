using Clinique_Projet.Controlers;
using Clinique_Projet.Modal;
using System;
using System.Windows;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Ajouter_Analyse.xaml
    /// </summary>
    public partial class Ajouter_Analyse : Window
    {
        public Ajouter_Analyse(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                Controlle_Analyse.Children.Add(new Parametre_Analyses(user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
    }
}
    