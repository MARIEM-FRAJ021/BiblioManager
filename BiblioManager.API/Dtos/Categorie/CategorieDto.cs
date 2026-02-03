using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Categorie
{
    public class CategorieDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Libelle { get; set; }
    }
}
