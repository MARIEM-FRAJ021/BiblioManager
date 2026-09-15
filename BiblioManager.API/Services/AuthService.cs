using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.Identity.Client;

namespace BiblioManager.API.Services
{
    public class AuthService : IAuthsService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailVerificationService _emailVerifService;

        public AuthService(IUtilisateurRepository utilisateurRepository, IJwtService jwtService, IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository, IEmailVerificationService emailVerifService)
        {
            _utilisateurRepository = utilisateurRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _emailVerifService = emailVerifService;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(request.Email) ??
                       throw new UnauthorizedAccessException("Credentials invalides");
            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Veuillez verifier votre adresse mail.");
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
            if (!storedToken.DateRevocation.HasValue)
            {
                storedToken.DateRevocation = DateTime.UtcNow;
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var user = new Utilisateur
            {
                Nom = registerDto.Nom,
                Prenom = registerDto.Prenom,
                Email = registerDto.Email,
                MotDePasse = BCrypt.Net.BCrypt.HashPassword(registerDto.MotDePasse),
                RoleUtilisateur = RoleUtilisateurEnum.Utilisateur,
                EmailConfirmed = false
            };
            await _utilisateurRepository.CreateAsync(user);
            await _emailVerifService.SendVerificationEmailAsync(user);
        }
    }
}
