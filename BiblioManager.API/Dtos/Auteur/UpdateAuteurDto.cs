using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Auteur
{
    public class UpdateAuteurDto
    {
        [Required, MaxLength(50)]
        public string Nom { get; set; }
        [Required, MaxLength(50)]
        public string Prenom { get; set; }
    }
}
