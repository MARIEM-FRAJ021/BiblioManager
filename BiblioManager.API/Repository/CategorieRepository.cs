using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class CategorieRepository : ICategorieRepository
    {
        private readonly BiblothequeDbContext _context;

        public CategorieRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categorie>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categorie?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Categorie> CreateAsync (Categorie categorie)
        {
            await _context.Categories.AddAsync(categorie);
            await _context.SaveChangesAsync();
            return categorie;
        }
    }
}
