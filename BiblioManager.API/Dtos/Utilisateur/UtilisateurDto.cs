using BiblioManager.API.Models;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.API.Dtos.Utilisateur
{
    public class UtilisateurDto
    {
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public RoleUtilisateurEnum RoleUtilisateur { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public Adherent Adherent { get; set; }
    }
}

