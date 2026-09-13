using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BiblioManager.API.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetByHashAsync(string tokenHash);
        Task SaveChangesAsync();
    }
}
