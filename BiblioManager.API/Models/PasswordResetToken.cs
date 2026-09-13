using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime DateExpiration { get; set; }
        public DateTime? DateUtilisation { get; set; }
        [ForeignKey("Utilisateur")]
        public int IdUtilisateur { get; set; }
        public Utilisateur? Utilisateur { get; set; }

    }
}
