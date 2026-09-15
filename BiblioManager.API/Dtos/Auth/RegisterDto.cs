using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auth
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required, MaxLength(50), EmailAddress, RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
        public string Email { get; set; } = string.Empty;

        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{1,12}$", ErrorMessage = "Le mot de passe doit contenir entre 1 et 12 caractères, au moins une majuscule, un chiffre et un caractère spécial.")]
        public string MotDePasse { get; set; } = string.Empty;
    }
}
