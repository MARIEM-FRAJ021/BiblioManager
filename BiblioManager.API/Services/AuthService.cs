using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Interfaces;

namespace BiblioManager.API.Services
{
    public class AuthService : IAuthsService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUtilisateurRepository utilisateurRepository, IJwtService jwtService)
        {
            _utilisateurRepository = utilisateurRepository;
            _jwtService = jwtService;
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
            return new AuthResponse
            {
                Token = accessToken,
                Expiration = DateTime.UtcNow.AddMinutes(30),
                UserId = user.IdUtilisateur,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Role = user.RoleUtilisateur.ToString()
            };

        }
    }
}
