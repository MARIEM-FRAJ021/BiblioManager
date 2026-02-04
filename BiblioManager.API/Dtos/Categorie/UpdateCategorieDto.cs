using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Categorie
{
    public class UpdateCategorieDto
    {
        [Required]
        [MaxLength(100)]
        public string Libelle { get; set; }
    }
}
