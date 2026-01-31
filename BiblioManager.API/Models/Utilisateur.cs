using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Models
{
    public class Utilisateur
    {
        [Key]
        public int IdUtilisateur { get; set; }

        [Required, MaxLength(50)]
        public string Nom { get; set; }

        [Required, MaxLength(50)]
        public string Prenom { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(12)]
        public string MotDePasse { get; set; }

        [Required]
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public Adherent Adherent { get; set; }

    }

    public enum RoleUtilisateurEnum
    {
        Admin,
        Employe,
        Adherent
    }
}
