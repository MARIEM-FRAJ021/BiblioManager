using BiblioManager.API.Models;
using Stripe.Checkout;

namespace BiblioManager.API.Interfaces
{
    public interface IPaiementService
    {
        Task<string> InitierPaiementCarte(TypePaiement type, int idUtilisateur);
        Task TraiterPaiementCarte(Session session);
    }
}
