using FluentValidation;
using System;

namespace Clinique_Projet.validation
{
    public class Validation_RendezVous : AbstractValidator<DateTime>
    {
        //ce class est pour valider les champs date
        public Validation_RendezVous(int champ)
        {

            switch (champ)
            {

                //utiliser dans la formulaire de ajouter un  patient
                case 1:
                    RuleFor(S => S)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                     .NotEmpty().WithMessage("le champs ne doit pas etre vide")
                     .Must(VAlideDate).WithMessage("la date est invalide");
                    break;
            }
        }

        //fonction pour valider que la date est inferieur a "date.now" et que le maximum age :130ans 

        private bool VAlideDate(DateTime Mydate)
        {
            int currentYear = DateTime.Now.Year;
            int MydateYear = Mydate.Year;
            if (Mydate < DateTime.Now && Mydate.ToShortDateString()!=DateTime.Now.ToShortDateString())
            {
               
                return false;
            }
            else return true;
            /* if ((Mydate.Day - DateTime.Now.Day <0) || Mydate.Year - DateTime.Now.Year < 0 || Mydate.Month - DateTime.Now.Month <0)
             {
                 return false;
             }
             return true;
         }*/

        }
    }
}
