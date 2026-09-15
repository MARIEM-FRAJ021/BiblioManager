using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auth
{
    public class VerifyEmailDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

    }
}
