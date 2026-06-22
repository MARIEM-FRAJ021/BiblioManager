using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BiblioManager.API.Repository
{
    public class EmpruntRepository : IEmpruntRepository
    {
        private readonly BiblothequeDbContext _context;
        public EmpruntRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task AjouterAsync(Emprunt emprunt)
        {
            await _context.Emprunts.AddAsync(emprunt);
        }

        public async Task<IEnumerable<Emprunt>> GetAllAsync()
        {
            return await _context.Emprunts.ToListAsync();
        }

        public async Task<Emprunt?> GetByIdAsync(int id)
        {
            return await _context.Emprunts
                .Include(e => e.Livre)
                .Include(e => e.Adherent)
                .FirstOrDefaultAsync(x => x.IdEmprunt == id);
        }

        public async Task<List<Emprunt>> GetEmpruntActifsByAdherents(int idAdherent)
        {
            return await _context.Emprunts
                .Where(e => e.IdAdherent == idAdherent && e.DateRetourEffective == null)
                .ToListAsync();
        }

        public async Task<List<Emprunt>> GetHistoriqueByAdherent(int idAdherent)
        {
            return await _context.Emprunts
                .Where(e => e.IdAdherent == idAdherent)
                .Include(e=>e.Adherent)
                .Include(e=> e.Livre)
                .ToListAsync();
        }

        public async Task<List<Emprunt>> GetEmpruntsEnRetard()
        {
            return await _context.Emprunts
                .Where(e => e.DateRetourEffective == null && e.DateRetourPrevue < DateTime.Now)
                .Include(e=>e.Adherent)
                .Include(e=>e.Livre)
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
