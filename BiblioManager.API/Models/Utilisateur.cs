using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Models
{
    public class Utilisateur
    {
        [Key]
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public Adherent Adherent { get; set; }
        /// <summary>
        /// Relation avec Paiements
        /// </summary>
        public ICollection<Paiement> Paiements { get; set; }
    }

    public enum RoleUtilisateurEnum
    {
        Admin,
        Employe,
        Utilisateur,
        Adherent
    }
}
