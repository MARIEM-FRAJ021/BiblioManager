using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Paiement
{
    public class CreatePaiementDto
    {
        [Required]
        public TypePaiement Type { get; set; } = TypePaiement.Abonnement;
    }
}
