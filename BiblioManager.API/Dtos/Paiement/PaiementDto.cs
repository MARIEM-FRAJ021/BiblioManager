using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Paiement
{
    public class PaiementDto
    {
        public int IdPaiement { get; set; }
        public int IdUtilisateur { get; set; }
        [Required]
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;
        public ModePaiementEnum Mode { get; set; } = ModePaiementEnum.Carte;
        // Pour paiement carte Stripe
        [Required]
        [MaxLength(255)]
        public string StripeSessionId { get; set; }= string.Empty;
        [Required]
        [MaxLength(100)]
        public string Reference { get; set; }=string.Empty;
        public PaiementStatutEnum Statut { get; set; }

        public TypePaiement Type { get; set; } = TypePaiement.Abonnement;
    }
}

