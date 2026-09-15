namespace BiblioManager.API.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetLink);

        Task SendEmailVerificationAsync (string email, string verificationLink);
    }
}
