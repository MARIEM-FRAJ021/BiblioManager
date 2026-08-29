using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Utilisateur
{
    public class UpdateUtilisateurDto
    {
        [Required, MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{1,12}$")]
        public string MotDePasse { get; set; } = string.Empty;
    }
}
