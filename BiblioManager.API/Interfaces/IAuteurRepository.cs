using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IAuteurRepository
    {
        Task<IEnumerable<Auteur>> GetAllAsync();
        Task<Auteur?> GetByIdAsync(int id);
        Task<Auteur> CreateAsync(Auteur auteur);
        Task<Auteur?> UpdateAsync(int id, Auteur auteur);
        Task<bool> DeleteAsync(int id);
    }
}
