using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly BiblothequeDbContext _context;
        public PasswordResetTokenRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
        }
        public async Task<PasswordResetToken?> GetByHashAsync(string tokenHash)
        {
            return await _context.PasswordResetTokens
                .Include(x => x.Utilisateur)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
