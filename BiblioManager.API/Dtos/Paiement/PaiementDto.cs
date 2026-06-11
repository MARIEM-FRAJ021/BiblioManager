using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BiblioManager.API.Dtos.Paiement
{
    public class PaiementDto
    {
        public int IdPaiement { get; set; }
        public int IdUtilisateur { get; set; }
        [Required]
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; } = DateTime.Now;
        public ModePaiementEnum Mode { get; set; } = ModePaiementEnum.Carte;
        // Pour paiement carte Stripe
        [Required]
        [MaxLength(255)]
        public string StripeSessionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Reference { get; set; }
        public PaiementStatutEnum Statut { get; set; }

        public TypePaiement Type { get; set; } = TypePaiement.Abonnement;
    }
}

