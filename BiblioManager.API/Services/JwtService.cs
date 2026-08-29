using BiblioManager.API.Configuration;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiblioManager.API.Services
{
    public class JwtService : IJwtService
    {
        public readonly JwtSettings _settings;
        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GenerateToken(Utilisateur user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{user.Nom} {user.Prenom}"),
                new Claim (ClaimTypes.Role, user.RoleUtilisateur.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.IdUtilisateur.ToString())
            };
            if (user.Adherent != null)
            {
                claims.Add(
                    new Claim(
                        "IdAdherent",
                        user.Adherent.IdAdherent.ToString()
                    )
                );
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               issuer: _settings.Issuer,
               audience: _settings.Audience,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
               signingCredentials: creds
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
