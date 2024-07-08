using Clinique_Projet.Controlers;
using Clinique_Projet.Modal;
using System;
using System.Windows;

namespace Clinique_Projet.forms
{
    /// <summary>
    /// Logique d'interaction pour Ajouter_medicament.xaml
    /// </summary>
    public partial class Ajouter_medicament : Window
    {
        public Ajouter_medicament(Utilisateur_Class  user)
        {
            try
            {
                InitializeComponent();
                Controlle_medicament.Children.Add(new Parametre_Medicament(user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }
    }
}
