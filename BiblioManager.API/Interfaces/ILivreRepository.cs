using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface ILivreRepository
    {
        Task<IEnumerable<Livre>> GetAllAsync();
        Task<Livre?> GetByIdAsync(int id);
        Task<Livre> CreateAsync(Livre livre);
        Task<Livre> UpdateAsync(int id,Livre livre);
        Task<bool> DeleteAsync(int id);

    }
}
