using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMAntecedents
    {
        public int patient_id { get; set; }
        public DateTime Date_Anteced { get; set; }
        public int TypeAtecd_id { get; set; }
        public string Descrip_Antecedent { get; set; }
    }
}
