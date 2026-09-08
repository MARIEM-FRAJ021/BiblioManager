using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;

namespace BiblioManager.API.Services
{
    public class AuthService : IAuthsService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUtilisateurRepository utilisateurRepository, IJwtService jwtService, IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _utilisateurRepository = utilisateurRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(request.Email) ??
                       throw new UnauthorizedAccessException("Credentials invalides");
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.MotDePasse))
                throw new UnauthorizedAccessException("Credentials invalides");
            if (user.Adherent != null && !user.Adherent.Actif)
                throw new InvalidOperationException("Votre compte est désactivé.");
            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = _refreshTokenService.HashRefreshToken(refreshToken),
                IdUtilisateur = user.IdUtilisateur,
                DateCreation = DateTime.UtcNow,
                DateExpiration = DateTime.UtcNow.AddDays(7)

            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();
            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7),
                UserId = user.IdUtilisateur,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Role = user.RoleUtilisateur.ToString()
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return;
            var hash = _refreshTokenService.HashRefreshToken(refreshToken);
            var storedToken = await _refreshTokenRepository.GetByHashAsync(hash);

            if (storedToken == null)
                return;
            if(!storedToken.DateRevocation.HasValue)
            {
                storedToken.DateRevocation = DateTime.UtcNow;
                await _refreshTokenRepository.SaveChangesAsync();
            }

        }
    }
}
