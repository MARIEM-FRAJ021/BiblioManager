using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Categorie;
using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioManager.API.Dtos.Livre
{
    public class LivreDto
    {
        public int IdLivre { get; set; }
        public string Titre { get; set; }
        public string ISBN { get; set; }
        public int QuantiteTotale { get; set; }
        public int QuantiteDisponible { get; set; }
        public AuteurDto Auteur { get; set; }
        public CategorieDto Categorie { get; set; }
    }
}
