using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{1,12}$", ErrorMessage = "Le mot de passe doit contenir entre 1 et 12 caractères, au moins une majuscule, un chiffre et un caractère spécial.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
