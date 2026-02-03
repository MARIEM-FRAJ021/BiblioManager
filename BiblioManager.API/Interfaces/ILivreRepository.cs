using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface ILivreRepository
    {
        Task<IEnumerable<Livre>> GetAllAsync();
        Task<Livre?> GetByIdAsync(int id);
        Task<Livre> CreateAsync(Livre livre);
        
    }
}
