using BiblioManager.API.Dtos;
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
                Type = paiement.Type
            };
        }

        public static Paiement ToPaiementFromCreatePaiementDto(this CreatePaiementDto createPaiementDto)
        {
            return new Paiement
            {
                IdUtilisateur = createPaiementDto.IdUtilisateur,
                StripeSessionId = createPaiementDto.StripeSessionId,
                Type = createPaiementDto.Type
            };
        }
    }
}