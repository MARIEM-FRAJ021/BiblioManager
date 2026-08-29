using BiblioManager.API.Models;

namespace BiblioManager.API.Interfaces
{
    public interface IUtilisateurService
    {
        Task ModifierRoleAsync(int idUtilisateur, RoleUtilisateurEnum role);
    }
}
