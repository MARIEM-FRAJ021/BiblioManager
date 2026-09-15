using BiblioManager.API.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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

        public async Task SendEmailVerificationAsync(string email, string verificationResetLink)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                "BiblioManager",
                _configuration["Email:From"]
                ));
            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Verification de votre adresse mail BiblioManager";
            message.Body = new TextPart("html")
            {

                Text = $"""
        <h2>Bienvenue à BiblioManager !</h2>

        <p>Merci beaucoup pour votre inscription !</p>

        <p>
            Veuillez vérifier votre adresse e-mail en cliquant sur le lien ci-dessous :
        </p>

        <p>
            <a href="{verificationResetLink}"
               style="display:inline-block; padding:12px 20px; background-color:#007bff; color:white; text-decoration:none; border-radius:5px;">
                Vérifier mon adresse e-mail
            </a>
        </p>

        <p>Ce lien expire dans 30 minutes.</p>

        <p>
            Si vous n'avez pas créé ce compte, vous pouvez ignorer cet e-mail.
        </p>
        """
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"]),SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
