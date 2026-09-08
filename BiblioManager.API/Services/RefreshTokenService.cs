using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace BiblioManager.API.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashRefreshToken(string refreshToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToBase64String(bytes);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException("Refresh token manquant.");
            var tokenHash = HashRefreshToken(refreshToken);
            var storedToken = await _refreshTokenRepository.GetByHashAsync(tokenHash);
            if (storedToken == null)
            {
                throw new UnauthorizedAccessException("refresh token invalide");
            }
            if (storedToken.DateRevocation.HasValue)
                throw new UnauthorizedAccessException("Refresh token révoqué");
            if (storedToken.DateExpiration <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expiré");
            var user = storedToken.Utilisateur;
            if (user == null)
                throw new UnauthorizedAccessException("Utilisateur introuvable");
            storedToken.DateRevocation = DateTime.UtcNow;

            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                TokenHash = HashRefreshToken(newRefreshToken),
                IdUtilisateur = storedToken.IdUtilisateur,
                DateCreation = DateTime.UtcNow,
                DateExpiration = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpiration = newRefreshTokenEntity.DateExpiration,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Role = user.RoleUtilisateur.ToString()
            };
        }
    }
}
