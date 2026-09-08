using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BiblothequeDbContext _context;
        public RefreshTokenRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens.Include(r => r.Utilisateur).FirstOrDefaultAsync(r => r.TokenHash == tokenHash);
        }
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
