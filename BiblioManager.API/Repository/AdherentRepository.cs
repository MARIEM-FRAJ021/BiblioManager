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

        public async Task<bool> UserIsAdherent(int idUtilisateur)
        {
            var userExists = await _context.Adherents.AnyAsync(x => x.IdUtilisateur == idUtilisateur);
            if (!userExists)
                return false;
            return true;
        }

        public async Task<IEnumerable<Adherent>> GetAdherentActifs()
        {
            return await _context.Adherents.Where(a => a.DateFin >= DateTime.Now).ToListAsync();
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
        public async Task<StatutAdherentEnum> GetStatutAdherent(int idAdherent)
        {
            var adherent = await _context.Adherents.FirstOrDefaultAsync(x => x.IdAdherent == idAdherent);
            if (adherent == null)
                return StatutAdherentEnum.NonAdherent;
            if (adherent?.DateFin < DateTime.Now)
                return StatutAdherentEnum.Expire;
            if (adherent?.DateFin >= DateTime.Now && adherent.Actif == false)
                return StatutAdherentEnum.Desactive;
            if (adherent?.Penalite > 0)
                return StatutAdherentEnum.PenaliteNonReglee;
            return StatutAdherentEnum.Actif;
        }
        public async Task AddAsync(Adherent adherent)
        {
            await _context.Adherents.AddAsync(adherent);
        }
        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}