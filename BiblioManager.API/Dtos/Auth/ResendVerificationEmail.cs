using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auth
{
    public class ResendVerificationEmail
    {
        [Required, MaxLength(50), EmailAddress, RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
        public string Email { get; set; } = string.Empty;
    }
}

