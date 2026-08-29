using BiblioManager.API.Models;

namespace BiblioManager.API.Dtos.Utilisateur
{
    public class UtilisateurDto
    {
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public AdherentDto? Adherent { get; set; }
    }
}

