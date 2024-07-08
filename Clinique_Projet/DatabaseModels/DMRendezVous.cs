using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMRendezVous
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public TimeSpan heure { get; set; }
        public int id_patient { get; set; }
        public string type { get; set; }
        public string description { get; set; }
    }

}
