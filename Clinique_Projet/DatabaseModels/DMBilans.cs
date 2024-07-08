using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMBilans
    {
        public int Consult_id { get; set; }
        public DateTime Date_Bilan { get; set; }
        public int Analyse_id { get; set; }
        public string Result_Analyse { get; set; }
    }

}
