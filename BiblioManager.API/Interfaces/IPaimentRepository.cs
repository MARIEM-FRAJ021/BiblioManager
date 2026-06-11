using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IPaimentRepository
    {
        Task<IEnumerable<Paiement>> GetPaiementsUtilisateur(int idUtilisateur);
        Task<Paiement?> GetById(int id);
    }
}
