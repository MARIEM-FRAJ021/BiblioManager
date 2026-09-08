using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime DateExpiration { get; set; }
        public DateTime? DateRevocation { get; set; }
        public bool IsRevoked => DateRevocation.HasValue;
        [ForeignKey("Utilisateur")]
        public int IdUtilisateur { get; set; }
        public Utilisateur? Utilisateur { get; set; }
    }
}
