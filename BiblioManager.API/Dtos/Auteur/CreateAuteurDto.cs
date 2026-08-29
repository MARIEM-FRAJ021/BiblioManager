using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auteur
{
    public class CreateAuteurDto
    {
        [Required, MaxLength(50)]
        public string Nom { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;
    }
}
