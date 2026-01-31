using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Adherent
    {
        [Key]
        public int IdAdherent { get; set; }
        [Required]
        [MaxLength(50)]
        public string Prenom {  get; set; }
        [Required]
        [MaxLength(50)]
        public string Nom { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool Actif { get; set; } = true;

        //Relation avec Emprunt
        public ICollection<Emprunt> Emprunts { get; set; }
        /// <summary>
        /// Relation avec Paiements
        /// </summary>
        public ICollection<Paiement> Paiements { get; set; }

        [ForeignKey("Utilisateur")]
        public int IdUtilisateur { get; set; }
        public Utilisateur Utilisateur { get; set; }

    }
}
