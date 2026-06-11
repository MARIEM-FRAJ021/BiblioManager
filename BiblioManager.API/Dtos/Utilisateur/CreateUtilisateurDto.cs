using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Utilisateur
{
    public class CreateUtilisateurDto
    {
        [Required, MaxLength(50)]
        public string Nom { get; set; }

        [Required, MaxLength(50)]
        public string Prenom { get; set; }

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }

        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{1,12}$", ErrorMessage = "Le mot de passe doit contenir entre 1 et 12 caractères, au moins une majuscule, un chiffre et un caractère spécial.")]
        public string MotDePasse { get; set; }
        [Required]
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
    }
}

