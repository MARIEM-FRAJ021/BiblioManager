using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Paiement
{
    public class CreatePaiementDto
    {
        [Required]
        public int IdUtilisateur { get; set; }
        [Required]
        [MaxLength(255)]
        public string StripeSessionId { get; set; }
        public TypePaiement Type { get; set; } = TypePaiement.Abonnement;
    }
}
