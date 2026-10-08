using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(int userId);
        Task SaveChangesAsync();
    }
}
