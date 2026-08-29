using BiblioManager.API.Dtos.Utilisateur;
using BiblioManager.API.Models;

namespace BiblioManager.API.Mappers
{
    public static class UtilisateurMapper
    {
        public static UtilisateurDto ToUtilisateurDto(this Utilisateur utilisateur)
        {
            return new UtilisateurDto
            {
                IdUtilisateur = utilisateur.IdUtilisateur,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Email = utilisateur.Email,
                RoleUtilisateur = utilisateur.RoleUtilisateur,
                DateCreation = utilisateur.DateCreation,
                Adherent = utilisateur.Adherent?.ToAdherentDto(),
            };
        }

        public static Utilisateur ToUtilisateurFromCreateUtilisateurDto(this CreateUtilisateurDto createUtilisateurDto)
        {
            return new Utilisateur
            {
                Nom = createUtilisateurDto.Nom,
                Prenom = createUtilisateurDto.Prenom,
                Email = createUtilisateurDto.Email,
                MotDePasse = BCrypt.Net.BCrypt.HashPassword(createUtilisateurDto.MotDePasse),
                RoleUtilisateur = RoleUtilisateurEnum.Utilisateur,
                DateCreation = DateTime.UtcNow.Date
            };
        }

        public static Utilisateur ToUtilisateurFromUpdateUtilisateurDto(this UpdateUtilisateurDto updateUtilisateurDto)
        {
            return new Utilisateur
            {
                Nom = updateUtilisateurDto.Nom,
                Prenom = updateUtilisateurDto.Prenom,
                Email = updateUtilisateurDto.Email,
                MotDePasse = BCrypt.Net.BCrypt.HashPassword(updateUtilisateurDto.MotDePasse),
                DateCreation = DateTime.UtcNow.Date
            };
        }
    }
}
