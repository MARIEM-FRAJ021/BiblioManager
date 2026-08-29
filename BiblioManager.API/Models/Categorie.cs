namespace BiblioManager.API.Models
{
    public class Categorie
    {
        public int Id { get; set; }

        public string Libelle { get; set; } = string.Empty;

        public ICollection<Livre> Livres { get; set; } = new List<Livre>();
    }
}
