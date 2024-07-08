using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMConsultation
    {
        public int id_Consult { get; set; }
        public string Motif_Consult { get; set; }
        public DateTime Date_Consult { get; set; }
        public string ExamenClinque_Consult { get; set; }
        public string Diagnostique_Consult { get; set; }
        public decimal Poids_patient { get; set; }
        public int Taille_patient { get; set; }
        public int Terminer_Consult { get; set; }
        public int Patient_id { get; set; }
    }

}
