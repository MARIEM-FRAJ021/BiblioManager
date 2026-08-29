using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Livre
    {
        [Key]
        public int IdLivre { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int QuantiteTotale { get; set; }
        public int QuantiteDisponible { get; set; }
        /// <summary>
        /// One-to-many
        /// </summary>
        [ForeignKey("Auteur")]
        public int AuteurId { get; set; }
        public Auteur? Auteur { get; set; }

        /// <summary>
        /// One-to-many
        /// </summary>
        [ForeignKey("Categorie")]
        public int IdCategorie { get; set; }
        public Categorie? Categorie { get; set; }
        public ICollection<Emprunt> Emprunts { get; set; } = new List<Emprunt>();


    }
}
