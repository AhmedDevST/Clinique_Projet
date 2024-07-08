using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinique_Projet.DatabaseModels
{
    public class DMPatient
    {
        public int id_patient { get; set; }
        public string Nom_patient { get; set; }
        public string Prenom_patient { get; set; }
        public string CIN_patient { get; set; }
        public DateTime DateNaissance { get; set; }
        public char Sex { get; set; }
        public string LieuNaissance { get; set; }
        public string Phone_patient { get; set; }
        public string Email_patient { get; set; }
        public string Address_patient { get; set; }
        public string Profession_patient { get; set; }
        public int Age_patient { get; set; }
        public DateTime DateSave_patient { get; set; }
        public string EtatCivil { get; set; }
        public int Assurance_id { get; set; }
        public int GroupSang_id { get; set; }
    }

}
