using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Categorie;

namespace BiblioManager.API.Dtos.Livre
{
    public class LivreDto
    {
        public int IdLivre { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int QuantiteTotale { get; set; }
        public int QuantiteDisponible { get; set; }
        public AuteurDto? Auteur { get; set; }
        public CategorieDto? Categorie { get; set; }
    }
}
