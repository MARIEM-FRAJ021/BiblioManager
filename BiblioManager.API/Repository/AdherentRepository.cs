using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BiblioManager.API.Repository
{
    public class AdherentRepository : IAdherentRepository
    {
        private readonly BiblothequeDbContext _context;

        public AdherentRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<Adherent?> GetAdherentById(int idAdherent)
        {
            return await _context.Adherents.FirstOrDefaultAsync(x => x.IdAdherent == idAdherent);
        }

        public async Task<IEnumerable<Adherent>> GetAdherents()
        {
            return await _context.Adherents.ToListAsync();
        }

        public async Task<bool> UserHasAdherent(int idUtilisateur)
        {
            var userExists = await _context.Adherents.AnyAsync(x => x.IdUtilisateur == idUtilisateur);
            if (!userExists)
                return false;
            return true;
        }

        public async Task<IEnumerable<Adherent>> GetAdherentActifs()
        {
            return await _context.Adherents.Where(a => a.Actif == true && a.DateFin >= DateTime.Now).ToListAsync();
        }

        public async Task UpdateAdherent(int id, Adherent adherentUpdateModel)
        {
            var existingAdherent = await _context.Adherents.FirstOrDefaultAsync(a => a.IdAdherent == id);
            if (existingAdherent == null)
                throw new Exception("Adherent introuvable");
            existingAdherent.Nom = adherentUpdateModel.Nom;
            existingAdherent.Prenom = adherentUpdateModel.Prenom;
            existingAdherent.Email = adherentUpdateModel.Email;
            await _context.SaveChangesAsync();
        }
    }
}