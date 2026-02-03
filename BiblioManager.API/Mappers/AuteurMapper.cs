using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class AuteurMapper
    {
        public static AuteurDto ToAuteurDto(this Auteur auteurModel)
        {
            return new AuteurDto
            {
                IdAuteur = auteurModel.IdAuteur,
                Nom = auteurModel.Nom,
                Prenom = auteurModel.Prenom,
            };
        }

        public static Auteur ToAuteurFromCreateAuteurDto(this CreateAuteurDto auteur)
        {
            return new Auteur
            {
                Nom = auteur.Nom,
                Prenom = auteur.Prenom,
            };
        }
    }
}
