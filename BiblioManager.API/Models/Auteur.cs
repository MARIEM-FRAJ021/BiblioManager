using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Models
{
    public class Auteur
    {
        [Key]
        public int IdAuteur { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        //Relation 1..* 
        public ICollection<Livre> Livres { get; set; } = new List<Livre>();
    }
}
