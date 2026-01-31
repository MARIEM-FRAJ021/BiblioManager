using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Livre
    {
        [Key]
        public int IdLivre { get; set; }
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
        /// <summary>
        /// One-to-many
        /// </summary>
        [ForeignKey("Auteur")]
        public int AuteurId { get; set; }
        public Auteur Auteur { get; set; }

        /// <summary>
        /// One-to-many
        /// </summary>
        [ForeignKey("Categorie")]
        public int IdCategorie { get; set; }
        public Categorie Categorie { get; set; }
        public ICollection<Emprunt> Emprunts { get; set; }


    }
}
