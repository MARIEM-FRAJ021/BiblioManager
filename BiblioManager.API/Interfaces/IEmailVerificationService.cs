using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IEmailVerificationService
    {
        Task SendVerificationEmailAsync(Utilisateur user);
        Task VerifyEmailAsync(string token);
        Task ResendVerificationEmailAsync(string email);
    }
}
