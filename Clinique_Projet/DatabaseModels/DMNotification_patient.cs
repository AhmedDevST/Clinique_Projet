using System;
namespace Clinique_Projet.DatabaseModels
{
    public class DMNotification_patient
    {
        public int id_notification { get; set; }
        public int patient_id { get; set; }
        public int user_id { get; set; }
        public string descriptionNotification { get; set; }
        public DateTime Date_notification { get; set; }
    }
}
