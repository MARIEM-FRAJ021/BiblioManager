using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Models
{
    public class Utilisateur
    {
        [Key]
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public Adherent? Adherent { get; set; }
        /// <summary>
        /// Relation avec Paiements
        /// </summary>
        public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
    }

    public enum RoleUtilisateurEnum
    {
        Admin,
        Employe,
        Utilisateur,
        Adherent
    }
}
