using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Emprunt
    {
        [Key]
        public int IdEmprunt { get; set; }

        /// <summary>
        /// Relation Adherent
        /// </summary>
        [Required]
        [ForeignKey("Adherent")]
        public int IdAdherent { get; set; }
        public Adherent Adherent { get; set; }

        /// <summary>
        /// Relation Livre
        /// </summary>
        [Required]
        [ForeignKey("Livre")]
        public int IdLivre { get; set; }
        public Livre Livre { get; set; }
        [Required]
        public DateTime DateEmprunt { get; set; }
        [Required]
        public DateTime DateRetourPrevue {  get; set; }
        public DateTime? DateRetourEffective { get; set; }

        public StatutEmprunt Statut = StatutEmprunt.EnCours;
    }

    public enum StatutEmprunt
    {
        EnCours,
        Retourne,
        EnRetard
    }
}
