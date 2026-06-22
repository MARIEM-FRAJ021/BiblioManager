using BiblioManager.API.Dtos.Emprunt;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class EmpruntMapper
    {
        public static EmpruntDto ToEmpruntDto(this Emprunt emprunt)
        {
            return new EmpruntDto
            {
                IdEmprunt = emprunt.IdEmprunt,
                IdLivre = emprunt.IdLivre,
                TitreLivre = emprunt.Livre?.Titre,
                IdAdherent = emprunt.IdAdherent,
                NomAdherent = $"{emprunt.Adherent.Nom} {emprunt.Adherent.Prenom}",
                DateEmprunt = emprunt.DateEmprunt,
                DateRetourEffective = emprunt.DateRetourEffective,
                DateRetourPrevue = emprunt.DateRetourPrevue,
                Statut = emprunt.Statut.ToString()
            };

        }
    }
}
