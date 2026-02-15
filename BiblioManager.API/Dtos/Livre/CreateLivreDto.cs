using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Dtos.Livre
{
    public class CreateLivreDto
    {
        [Required]
        [MaxLength(200)]
        public string Titre { get; set; }
        [MaxLength(20)]
        public string ISBN { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int QuantiteTotale { get; set; }
        public int QuantiteDisponible => QuantiteTotale;
        [Required]
        public int AuteurId { get; set; }
        [Required]
        public int IdCategorie { get; set; }
    }
}
