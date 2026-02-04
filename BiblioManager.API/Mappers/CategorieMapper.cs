using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Categorie;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class CategorieMapper
    {
        public static CategorieDto ToCategorieDto(this Categorie categ)
        {
            return new CategorieDto
            {
                Id = categ.Id,
                Libelle = categ.Libelle,
            };
        }

        public static Categorie ToCategorieFromCreateCategorieDto(this CreateCategorieDto categ)
        {
            return new Categorie
            {
                Libelle = categ.Libelle,
            };
        }

        public static Categorie ToCategorieFromUpdateCategorieDto(this UpdateCategorieDto categ)
        {
            return new Categorie
            {
                Libelle = categ.Libelle,
            };
        }
    }
}
