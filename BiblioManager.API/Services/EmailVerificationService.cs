using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace BiblioManager.API.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailVerificationTokenRepository _tokenRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;


        public EmailVerificationService(IEmailService emailService, IEmailVerificationTokenRepository tokenRepository, IUtilisateurRepository utilisateurRepository)
        {
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task SendVerificationEmailAsync(Utilisateur user)
        {
            var token = GenerateEmailVerificationToken();
            var tokenHash = HashToken(token);

            var now = DateTime.UtcNow;

            var verificationToken = new EmailVerificationToken
            {
                TokenHash = tokenHash,
                IdUtilisateur = user.IdUtilisateur,
                DateCreation = now,
                DateExpiration = now.AddMinutes(30)
            };

            await _tokenRepository.AddAsync(verificationToken);
            await _tokenRepository.SaveChangesAsync();

            var verificationLink = $"http://localhost:4200/verify-email?token=" +
            $"{Uri.EscapeDataString(token)}";

            await _emailService.SendEmailVerificationAsync(user.Email, verificationLink);
        }

        public async Task VerifyEmailAsync(string token)
        {
            var tokenHash = HashToken(token);
            var verificationToken = await _tokenRepository.GetByHashAsync(tokenHash);

            if (verificationToken == null)
            {
                throw new UnauthorizedAccessException("Token invalide");
            }

            if (verificationToken.DateUtilisation.HasValue)
            {
                throw new UnauthorizedAccessException("Ce token a déjà été utilisé.");
            }

            if (verificationToken.DateExpiration <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Ce token a expiré");
            }

            var user = verificationToken.Utilisateur;

            if (user.EmailConfirmed)
            {
                return;
            }

            user.EmailConfirmed = true;

            verificationToken.DateUtilisation = DateTime.UtcNow;
            await _tokenRepository.SaveChangesAsync();
        }

        public async Task ResendVerificationEmailAsync(string email)
        {
            var utilisateur = await _utilisateurRepository.GetByEmailAsync(email);
            if (utilisateur == null)
                throw new KeyNotFoundException("Utilisateur introuvable");
            if (utilisateur.EmailConfirmed)
                throw new KeyNotFoundException("Cette adresse e-mail est déjà vérifiée.");

            var ancienEmailVerificationTokens = await _tokenRepository.GetEmailVerifTokensByUser(utilisateur.IdUtilisateur);
            ancienEmailVerificationTokens.ForEach(t => t.DateUtilisation = DateTime.UtcNow);

            var token = GenerateEmailVerificationToken();
            var tokenHash= HashToken(token);

            var verificationToken = new EmailVerificationToken
            {
                TokenHash = tokenHash,
                IdUtilisateur = utilisateur.IdUtilisateur,
                DateCreation = DateTime.UtcNow,
                DateExpiration = DateTime.UtcNow.AddMinutes(30)
            };

            await _tokenRepository.AddAsync(verificationToken);
            await _tokenRepository.SaveChangesAsync();

            var verificationLink = $"http://localhost:4200/verify-email?token=" +
            $"{Uri.EscapeDataString(token)}";

            await _emailService.SendEmailVerificationAsync(utilisateur.Email, verificationLink);

        }

        private string GenerateEmailVerificationToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return WebEncoders.Base64UrlEncode(randomBytes);
        }

        private string HashToken(string token)
        {
            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
