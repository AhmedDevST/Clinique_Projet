using Clinique_Projet.Modal;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Clinique_Projet.Controlers
{
    /// <summary>
    /// Logique d'interaction pour More_Parametre.xaml
    /// </summary>
    public partial class More_Parametre : UserControl
    {
        public TypeAntecedent Obj_TypeAntecdent;
        public AssuranceClass Obj_Assurance;
        public GroupSangClass Obj_GroupSang;
        public Utilisateur_Class user;
        public More_Parametre(Utilisateur_Class user)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                Initialiser_TabItems();
            }
            catch(Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }       
        //-----------------------  SATRT  METHODES  :  ------------------------------------ ----

        private void Initialiser_TabItems()
        {
            try
            {
                Controle_Parametres_assurance.Children.Clear();
                Controle_Parametres_assurance.Children.Add(new Parametre_Agent_Assurance(user));

                Controle_Parametres_antecdents.Children.Clear();
                Controle_Parametres_antecdents.Children.Add(new Prametre_Antecedent_Control(user));

                Controle_Parametres_GroupSang.Children.Clear();
                Controle_Parametres_GroupSang.Children.Add(new Parametre_Group_Sang(user));
            }
            catch (Exception)
            {
                MessageBox.Show("Opération d'entrée innatendu !!");
            }
        }

        //-----------------------  END  METHODES  :  ------------------------------------ ----
    }
}
