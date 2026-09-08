using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }
}
