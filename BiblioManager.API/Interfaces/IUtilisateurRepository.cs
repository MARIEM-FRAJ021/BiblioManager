using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IUtilisateurRepository
    {
        Task<IEnumerable<Utilisateur>> GetAllAsync();
        Task<Utilisateur?> GetByIdAsync(int id);
        Task<Utilisateur?> GetByEmailAsync(string email);
        Task<Utilisateur> CreateAsync(Utilisateur user);
        Task<Utilisateur?> UpdateAsync(int id, Utilisateur user);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();

    }
}
