using System;

namespace Clinique_Projet.DatabaseModels
{
    public class DMUtilisateur
    {
        public int id_user { get; set; }
        public string Nom_user { get; set; }
        public string Prenom_user { get; set; }
        public string CIN_user { get; set; }
        public string Phone_user { get; set; }
        public string Email_user { get; set; }
        public string Diplome_user { get; set; }
        public byte[] Photo_user { get; set; }
        public int Status_Compte { get; set; } // activer non activer
        public int IsAdmin { get; set; }
        public int IsConnect { get; set; }
        public int Role_user { get; set; }
        public string Login_user { get; set; }
        public string password_user { get; set; }

    }
}
