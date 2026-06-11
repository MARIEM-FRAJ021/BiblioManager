using BiblioManager.API.Dtos;
using BiblioManager.API.Dtos.Utilisateur;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class AdherentMapper
    {
        public static AdherentDto ToAdherentDto(this Adherent adherent)
        {
            return new AdherentDto
            {
                IdAdherent = adherent.IdAdherent,
                Nom = adherent.Nom,
                Prenom = adherent.Prenom,
                Email = adherent.Email,
                DateDebut = adherent.DateDebut,
                DateFin = adherent.DateFin,
                Penalite = adherent.Penalite,
                Emprunts = adherent.Emprunts?.Select(x=> x.ToEmpruntDto()).ToList()
            };
        }
        public static Adherent ToAdherentFromAdherentDto(this AdherentDto adherent)
        {
            return new Adherent
            {
                IdAdherent = adherent.IdAdherent,
                IdUtilisateur = adherent.IdUtilisateur,
                Nom = adherent.Nom,
                Prenom = adherent.Prenom,
                Email = adherent.Email,
                DateDebut = adherent.DateDebut,
                DateFin = adherent.DateFin,
                Penalite = adherent.Penalite
                //Emprunts = adherent.Emprunts?.Select(x => x.ToEmpruntDto()).ToList()
            };
        }

    }
}
