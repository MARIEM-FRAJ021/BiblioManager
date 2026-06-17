using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IAdherentRepository
    {
        Task<Adherent?> GetAdherentById(int idAdherent);
        Task<IEnumerable<Adherent>> GetAdherents();
        Task<bool> UserIsAdherent(int idUtilisateur);
        Task<IEnumerable<Adherent>> GetAdherentActifs();
        Task UpdateAdherent(int id, Adherent adherentUpdateModel);
        Task AddAsync(Adherent adherent);
        Task SaveChangesAsync();
        Task<StatutAdherentEnum> GetStatutAdherent(int idAdherent);
    }
}
