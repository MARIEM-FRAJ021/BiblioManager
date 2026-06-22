using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IEmpruntRepository
    {
        Task<IEnumerable<Emprunt>> GetAllAsync();
        Task AjouterAsync(Emprunt emprunt);
        Task<Emprunt?> GetByIdAsync(int id);
        Task<List<Emprunt>> GetEmpruntActifsByAdherents(int idAdherent);
        Task<List<Emprunt>> GetEmpruntsEnRetard();
        Task<List<Emprunt>> GetHistoriqueByAdherent(int idAdherent);
        Task SaveChangesAsync(); 
    }
}
