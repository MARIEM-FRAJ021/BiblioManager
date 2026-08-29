using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;

namespace BiblioManager.API.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _userRepo;

        public UtilisateurService(IUtilisateurRepository utilisateurRepository)
        {
            _userRepo = utilisateurRepository;
        }

        public async Task ModifierRoleAsync(int idUtilisateur, RoleUtilisateurEnum nouveauRole)
        {
            var utilisateur = await _userRepo.GetByIdAsync(idUtilisateur);

            if (utilisateur == null)
                throw new KeyNotFoundException("Utilisateur introuvable.");

            // Le rôle Adherent est géré par la logique d'adhésion
            if (nouveauRole == RoleUtilisateurEnum.Adherent)
                throw new InvalidOperationException("Le rôle Adherent est attribué via la procédure d'adhésion.");

            utilisateur.RoleUtilisateur = nouveauRole;

            await _userRepo.SaveChangesAsync();
        }

    }
}
