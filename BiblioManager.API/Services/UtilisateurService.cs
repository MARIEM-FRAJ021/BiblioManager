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
            
            
            utilisateur.RoleUtilisateur = nouveauRole;

            await _userRepo.SaveChangesAsync();
        }

    }
}
