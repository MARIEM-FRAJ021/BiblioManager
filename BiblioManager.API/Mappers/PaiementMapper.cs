using BiblioManager.API.Dtos.Paiement;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class PaiementMapper
    {
        public static PaiementDto ToPaiementDto(this Paiement paiement)
        {
            return new PaiementDto
            {
                IdPaiement = paiement.IdPaiement,
                IdUtilisateur = paiement.IdUtilisateur,
                Montant = paiement.Montant,
                DatePaiement = paiement.DatePaiement,
                Mode = paiement.Mode,
                StripeSessionId = paiement.StripeSessionId,
                Reference = paiement.Reference,
                Type = paiement.Type,
                Statut =paiement.Statut
            };
        }
    }
}