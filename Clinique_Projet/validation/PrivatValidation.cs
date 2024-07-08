using Clinique_Projet.Modal;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clinique_Projet.validation
{
    // dans ce class :regles de validation de plusieurs champs selon une valeur
    //(nom,prenom,cin,email,phone,address
    public class PrivatValidation: AbstractValidator<String>
    {
        [Obsolete]
        /*
            1:nom,prenom
            2:CIN
            3:email
            4:telephone
         */
        public PrivatValidation( int champ ) {
            switch (champ)
            {
                //nom prenom(alpha)
                case 1:
                    RuleFor(S => S)
                      .Cascade(CascadeMode.StopOnFirstFailure)
                      .NotEmpty().WithMessage("le champs ne doit pas etre vide")
                      .Must(VAlideName).WithMessage("le champs  content invalide character")
                      .Length(2, 50).WithMessage("la longeur de ce champs est invalide");
                    break;
                
                    //  cin (pas la test de unicite) 
                case 2:
                    RuleFor(S => S)
                      .Cascade(CascadeMode.StopOnFirstFailure)
                      .NotEmpty().WithMessage("le CIN ne doit pas etre vide")
                      .Must(x => x.All(char.IsLetterOrDigit)).WithMessage("le champs  content invalide characters .")
                     .Length(3, 20).WithMessage("la longeur de ce champs est invalide");
                    break;

                    //email
                case 3:
                    RuleFor(S => S)
                     .Cascade(CascadeMode.StopOnFirstFailure)
                     .EmailAddress().WithMessage("L'email address est invalide");
                    break;

                    //phone
                case 4:
                    RuleFor(S => S)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(phone => phone.All(char.IsDigit)).WithMessage("le téléphone doit contient seulement des nombres.")
                    .Length(10).WithMessage("le téléphone doit contient  10 nombres.");
                    break;
                
                    //addres (alpha numerique)
                case 5:
                    RuleFor(S => S)
                    .Length(2,500).WithMessage("la longeur de ce champs est invalide.");
                    break;

                  //  ce regle est pour valider si le  cin est  UNIQUE
                case 6:
                    RuleFor(S => S)
                      .Cascade(CascadeMode.StopOnFirstFailure)
                      .NotEmpty().WithMessage("le CIN ne doit pas etre vide")
                      .Must(x => x.All(char.IsLetterOrDigit)).WithMessage("le champs  content invalide characters .")
                      .Length(3, 20).WithMessage("la longeur de CIN est invalide")
                     .Must(BeUniqueCIN).WithMessage("le CIN doit etre unique");
                    break;

                // valide nombre numeriques
                case 7:
                    RuleFor(S => S)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(x => decimal.TryParse(x, out decimal result) && result > 0).WithMessage("entre a valide nombre.")
                     .MaximumLength(7).WithMessage("la longeur de ce champs est invalide");
                    break;

                //valide int number (quantite de medicament)
                case 8:
                    RuleFor(vm => vm)
                   .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("Le champs ne doit etre pas vide!")
                    .Must(x => int.TryParse(x, out int result) && result > 0).WithMessage("entre a valide nombre.");
                    break;

                // valide nombre int
                case 9:
                    RuleFor(S => S)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .Must(x => int.TryParse(x, out int result) && result > 0).WithMessage("entre a valide nombre.");
                    break;
                // 
                case 10:
                    RuleFor(S => S)
                      .Cascade(CascadeMode.StopOnFirstFailure)
                      .NotEmpty().WithMessage("le champs ne doit pas etre vide")
                      .Length(2, 2000).WithMessage("la longeur de ce champs est invalide");
                    break;

                //MOT DE PASSE (alpha numerique)
                case 11:
                    RuleFor(S => S)
                    .MinimumLength(6).WithMessage("la longeur de ce champs est invalide.");
                    break;

                // must phone
                case 12:
                    RuleFor(S => S)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                     .NotEmpty().WithMessage("le telephone ne doit pas etre vide")
                    .Must(phone => phone.All(char.IsDigit)).WithMessage("le téléphone doit contient seulement des nombres.")
                    .Length(10).WithMessage("le téléphone doit contient  10 nombres.");
                    break;
               
            }
        }

        // pour valide les nom,prenom doit containe seulement les lettres
        private bool VAlideName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }
        
        //ce fonction pour valider si le CIN est unique ou non
        private bool BeUniqueCIN(string cin)
        {
            List<string> cins = PatientClass.DisplayCIN_Patient();  //select a partie de database list de tous les CIN;
            foreach (string c in cins)
            {
                if (c.ToUpper() == cin.ToUpper()) return false;
            }
            return true;
        }


    }
}
