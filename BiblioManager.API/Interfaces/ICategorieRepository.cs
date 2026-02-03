using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface ICategorieRepository
    {
        Task<IEnumerable<Categorie>> GetAllAsync();
        Task<Categorie?> GetByIdAsync(int id);
        Task<Categorie> CreateAsync(Categorie categorie);
    }
}
