using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IPaimentRepository
    {
        Task<IEnumerable<Paiement>> GetPaiementsUtilisateur(int idUtilisateur);
        Task<Paiement?> GetById(int id);
        Task<Paiement?> GetDernierPaiementValide(int idUtilisateur);
        Task<Paiement?> GetPaiementToTreat(string stripeSessionId);
        Task AddAsync(Paiement paiement);
        Task SaveChangesAsync();
    }
}
