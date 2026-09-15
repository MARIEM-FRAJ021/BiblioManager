using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddAsync(EmailVerificationToken token);
        Task<EmailVerificationToken?> GetByHashAsync(string tokenHash);
        Task<List<EmailVerificationToken>> GetEmailVerifTokensByUser(int userId);
        Task SaveChangesAsync();
    }
}
