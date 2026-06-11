using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IPaiementService
    {
        Task<Paiement> InitierPaiementCarte(Paiement paiement);
        Task TraiterPaiementCarte(string stripeSessionId, bool paiementReussi);
    }
}
