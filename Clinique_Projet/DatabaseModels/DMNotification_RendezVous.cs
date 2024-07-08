using System;

namespace Clinique_Projet.DatabaseModels
{
    public class DMNotification_RendezVous
    {
        public  int id_notification { get; set; }
        public int rdv_id { get; set; } 
        public int user_id { get; set; }
        public string descriptionNotification  { get; set; }
        public DateTime Date_notification { get; set; }
    }
}
