using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
    {
        private readonly BiblothequeDbContext _context;
        public EmailVerificationTokenRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmailVerificationToken>> GetEmailVerifTokensByUser(int userId)
        {
            return await _context.EmailVerificationTokens.Where(t =>
                t.IdUtilisateur == userId &&
                !t.DateUtilisation.HasValue).ToListAsync();
        }

        public async Task AddAsync(EmailVerificationToken token)
        {
            await _context.EmailVerificationTokens.AddAsync(token);
        }

        public async Task<EmailVerificationToken?> GetByHashAsync(string tokenHash)
        {
            return await _context.EmailVerificationTokens.Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
