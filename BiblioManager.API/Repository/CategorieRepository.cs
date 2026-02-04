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

        public async Task<Categorie?> UpdateAsync(int id, Categorie categorieUpdateModel)
        {
            var categorieModel = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (categorieModel == null)
                return null;
            categorieModel.Libelle = categorieUpdateModel.Libelle;
            await _context.SaveChangesAsync();
            return categorieModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(id==1)
            {
                throw new Exception("Impossible de supprimer cette catégorie.");
            }
            var categorieModel = await _context.Categories.Include(c=>c.Livres).FirstOrDefaultAsync(x => x.Id == id);

            if (categorieModel != null)
            {
                foreach (var livre in categorieModel.Livres)
                {
                    livre.IdCategorie = 1;
                }
                _context.Categories.Remove(categorieModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
