using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class AuteurRepository : IAuteurRepository
    {
        private readonly BiblothequeDbContext _context;

        public AuteurRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auteur>> GetAllAsync()
        {
            return await _context.Auteurs.ToListAsync();
        }

        public async Task<Auteur?> GetByIdAsync(int id)
        {
            var auteur = await _context.Auteurs.FirstOrDefaultAsync(x => x.IdAuteur == id);
            return auteur;
        }

        public async Task<Auteur> CreateAsync(Auteur auteurModel)
        {
            await _context.Auteurs.AddAsync(auteurModel);
            await _context.SaveChangesAsync();
            return auteurModel;
        }

        public async Task<Auteur?> UpdateAsync(int id, Auteur auteurUpdateModel)
        {
            var auteurModel = await _context.Auteurs.FirstOrDefaultAsync(x => x.IdAuteur == id);
            if (auteurModel == null)
                return null;
            auteurModel.Nom = auteurUpdateModel.Nom;
            auteurModel.Prenom = auteurUpdateModel.Prenom;
            await _context.SaveChangesAsync();
            return auteurModel;
        }

        public async Task<bool> DeleteAsync (int id)
        {
            var auteurModel = await _context.Auteurs.Include(a=>a.Livres).FirstOrDefaultAsync(x => x.IdAuteur == id);

            if (auteurModel != null)
            {
                if (auteurModel.Livres.Any())
                    throw new Exception("Impossible de supprimer cet auteur car il a des livres associés.");
                _context.Auteurs.Remove(auteurModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
