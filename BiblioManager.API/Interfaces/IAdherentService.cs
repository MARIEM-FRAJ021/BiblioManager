using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IAdherentService
    {
        Task DevenirAdherent(Paiement paiement);
        Task VerifierAdherentActif(int idAdherent);
        Task RenouvelerAbonnement(Paiement paiement);
        Task<bool> DesactiverAdhesion(int idAdherent);
    }
}
