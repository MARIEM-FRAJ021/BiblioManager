using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface ICategorieRepository
    {
        Task<IEnumerable<Categorie>> GetAllAsync();
        Task<Categorie?> GetByIdAsync(int id);
        Task<Categorie> CreateAsync(Categorie categorie);
        Task<Categorie?> UpdateAsync(int id, Categorie categorie);
        Task<bool> DeleteAsync(int id);
    }
}
