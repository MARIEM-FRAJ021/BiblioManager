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

        public async Task<IEnumerable<Livre>> GetAllAsync ()
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
    }
}
