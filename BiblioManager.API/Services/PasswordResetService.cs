using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace BiblioManager.API.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUtilisateurRepository _utiliateurRepository;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailService _emailService;

        public PasswordResetService(IUtilisateurRepository utilisateurRepository, IPasswordResetTokenRepository passwordResetTokenRepository, IEmailService emailService)
        {
            _utiliateurRepository = utilisateurRepository;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailService = emailService;
        }
        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _utiliateurRepository.GetByEmailAsync(email);
            if (user == null)
                return;
            var token = GenerateResetPasswordToken();
            var tokenHash = HashResetPasswordToken(token);
            var now = DateTime.UtcNow;
            var resetToken = new PasswordResetToken
            {
                TokenHash = tokenHash,
                IdUtilisateur = user.IdUtilisateur,
                DateCreation = now,
                DateExpiration = now.AddMinutes(30),
            };
            await _passwordResetTokenRepository.AddAsync(resetToken);
            await _passwordResetTokenRepository.SaveChangesAsync();
            var resetLink = $"http://localhost:4200/reset-password?token={Uri.EscapeDataString(token)}";

            await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
        }

        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var tokenHash = HashResetPasswordToken(token);
            var resetToken = await _passwordResetTokenRepository.GetByHashAsync(tokenHash);

            if (resetToken == null)
                throw new UnauthorizedAccessException("Token invalide");
            if (resetToken.DateUtilisation.HasValue)
                throw new UnauthorizedAccessException("Ce token a déjà été utilisé");
            if (resetToken.DateExpiration <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Ce token a expiré.");
            var user = resetToken.Utilisateur;
            if (user == null)
                throw new UnauthorizedAccessException("utilisateur introuvable");
            user.MotDePasse = BCrypt.Net.BCrypt.HashPassword(newPassword);
            resetToken.DateUtilisation = DateTime.UtcNow;

            await _passwordResetTokenRepository.SaveChangesAsync();
        }

        private string GenerateResetPasswordToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return WebEncoders.Base64UrlEncode(randomBytes);
        }

        private string HashResetPasswordToken(string token)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
