using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMOrdonnance
    {
        public int Consult_id { get; set; }
        public int Medicament_id { get; set; }
        public DateTime Date_Ordonnace { get; set; }
        public string Posologie { get; set; }
        public string Note_Supplimentaire { get; set; }
        public int Quantite { get; set; }
    }

}
