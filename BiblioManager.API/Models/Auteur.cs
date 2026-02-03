using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Models
{
    public class Auteur
    {
        [Key]
        public int IdAuteur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        //Relation 1..* 
        public ICollection<Livre> Livres { get; set; }


    }
}
