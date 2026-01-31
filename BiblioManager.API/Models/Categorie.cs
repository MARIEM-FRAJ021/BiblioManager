namespace BiblioManager.API.Models
{
    public class Categorie
    {
        public int Id { get; set; }

        public string Libelle { get; set; }

        public ICollection<Livre> Livres { get; set; }
    }
}
