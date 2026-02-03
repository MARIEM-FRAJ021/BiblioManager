using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Livre
{
    public class UpdateLivreDto
    {
        [Required]
        [MaxLength(200)]
        public string Titre { get; set; }
        [MaxLength(20)]
        public string ISBN { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int QuantiteTotale { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int QuantiteDisponible { get; set; }
        [Required]
        public int AuteurId { get; set; }
        [Required]
        public int IdCategorie { get; set; }
    }
}
