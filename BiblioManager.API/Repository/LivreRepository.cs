using BiblioManager.API.DAL;
using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class LivreRepository : ILivreRepository
    {
        private readonly BiblothequeDbContext _context;

        public LivreRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Livre>> GetAllAsync()
        {
            return await _context.Livres.Include(l => l.Categorie)
                .Include(l => l.Auteur).ToListAsync();
        }

        public async Task<Livre?> GetByIdAsync(int id)
        {
            var livre = await _context.Livres.Include(l => l.Auteur).Include(l => l.Categorie).FirstOrDefaultAsync(x => x.IdLivre == id);
            return livre;
        }

        public async Task<Livre> CreateAsync(Livre livreModel)
        {
            await _context.Livres.AddAsync(livreModel);
            await _context.SaveChangesAsync();
            return livreModel;
        }

        public async Task<Livre> UpdateAsync(int id, Livre livreUpdateModel)
        {
            var livreModel = await _context.Livres.Include(l => l.Emprunts).FirstOrDefaultAsync(x => x.IdLivre == id);
            if (livreModel == null)
                return null;
            livreModel.Titre = livreUpdateModel.Titre;
            livreModel.ISBN = livreUpdateModel.ISBN;
            livreModel.QuantiteTotale = livreUpdateModel.QuantiteTotale < livreModel.Emprunts.Count() ? throw new Exception("La quantité totale doit être supérieure ou égale aux emprunts") : livreUpdateModel.QuantiteTotale;
            livreModel.QuantiteDisponible = livreModel.QuantiteTotale - livreModel.Emprunts.Count(e => e.DateRetourEffective > DateTime.Now);
            livreModel.AuteurId = livreUpdateModel.AuteurId;
            await _context.SaveChangesAsync();
            return livreModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var livreModel = await _context.Livres.Include(c => c.Emprunts).FirstOrDefaultAsync(x => x.IdLivre == id);
            if (livreModel != null)
            {
                if (livreModel.Emprunts.Any())
                {
                    throw new Exception("Vous ne pouvez pas supprimer un livre ayant des emprunts.");
                }
                _context.Livres.Remove(livreModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
