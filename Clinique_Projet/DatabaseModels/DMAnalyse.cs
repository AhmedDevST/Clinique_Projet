using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMAnalyse
    {
        public int id_Analyse { get; set; }
        public string Nom_Analyse { get; set; }
        public int Type_Analyse_id { get; set; }
    }

}
