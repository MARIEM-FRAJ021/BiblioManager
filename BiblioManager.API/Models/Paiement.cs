using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }

        [Required]
        [ForeignKey("Adherent")]
        public int IdAdherent { get; set; }
        public Adherent Adherent { get; set; }
        [Required]
        public decimal Montant { get; set; }

        [Required]
        public DateTime DatePaiement { get; set; } = DateTime.Now;

        [Required]
        public ModePaiementEnum Mode { get; set; }
    }

    public enum ModePaiementEnum
    {
        Especes,
        Carte,
        Virement
    }
}
