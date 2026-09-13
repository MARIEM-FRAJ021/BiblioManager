using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auth
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
