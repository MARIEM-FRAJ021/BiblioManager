using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Categorie
{
    public class CreateCategorieDto
    {
        [Required]
        [MaxLength(100)]
        public string Libelle { get; set; } = string.Empty;
    }
}
