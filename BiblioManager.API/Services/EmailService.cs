using BiblioManager.API.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;


namespace BiblioManager.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
               "BiblioManager", _configuration["Email:From"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Reinitialisation de votre mot de passe";

            var body = $"""
                Bonjour,
                Vous avez demander la réinitialisation de votre mot de passe BiblioManager.

                Cliquez sur le lien suivant :
                {resetLink}
                Ce lien expire dans 30 minutes.

                Si vous n'êtes pas à l'origine de cette demande,
                vous pouvez ignorer cet email.

                BiblioManager
                """;

            message.Body = new TextPart("plain")
            {
                Text = body,
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["Email:SmtpServer"]!,
                int.Parse(_configuration["Email:Port"]!),
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Username"]!, _configuration["Email:Password"]!);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
