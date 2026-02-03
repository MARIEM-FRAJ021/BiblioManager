using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class LivreMapper
    {
        public static LivreDto ToLivreDto(this Livre livreModel)
        {
            return new LivreDto
            {
                IdLivre = livreModel.IdLivre,
                Titre = livreModel.Titre,
                ISBN = livreModel.ISBN,
                QuantiteTotale = livreModel.QuantiteTotale,
                QuantiteDisponible = livreModel.QuantiteDisponible,
                Auteur = livreModel.Auteur.ToAuteurDto(),
                Categorie = livreModel.Categorie.ToCategorieDto()
            };
        }

        public static Livre ToLivreFromCreateLivreDto(this CreateLivreDto createLivreDto)
        {
            return new Livre
            {
                Titre = createLivreDto.Titre,
                ISBN = createLivreDto.ISBN,
                QuantiteTotale = createLivreDto.QuantiteTotale,
                QuantiteDisponible = createLivreDto.QuantiteDisponible,
                AuteurId = createLivreDto.AuteurId,
                IdCategorie = createLivreDto.IdCategorie
            };
        }
    }
}
