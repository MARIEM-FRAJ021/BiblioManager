using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Repository
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly BiblothequeDbContext _context;

        public UtilisateurRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await _context.Utilisateurs.Include(u =>u.Adherent).ToListAsync();
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            return await _context.Utilisateurs.Include(x=>x.Adherent).FirstOrDefaultAsync(x => x.IdUtilisateur == id);
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur user)
        {
            var users = _context.Utilisateurs;
            if (user.RoleUtilisateur == RoleUtilisateurEnum.Admin && users.Any(u => u.RoleUtilisateur == RoleUtilisateurEnum.Admin))
                throw new Exception("Impossible : Un utilisateur admin existe déjà.");
            await users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Utilisateur?> UpdateAsync(int id, Utilisateur user)
        {
            var users = await _context.Utilisateurs.ToListAsync();
            var userModel =  users.FirstOrDefault(x => x.IdUtilisateur == id);
            if (userModel == null)
                return null;
            if(user.RoleUtilisateur == RoleUtilisateurEnum.Admin && users.Any(x=> x.RoleUtilisateur == RoleUtilisateurEnum.Admin))
                throw new Exception("Impossible : Un utilisateur admin existe déjà.");
            userModel.Nom = user.Nom;
            userModel.Prenom = user.Prenom;
            userModel.MotDePasse = user.MotDePasse;
            userModel.Email = user.Email;
            userModel.RoleUtilisateur = user.RoleUtilisateur;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var utilisateurModel = await _context.Utilisateurs.Include(a => a.Adherent).FirstOrDefaultAsync(x => x.IdUtilisateur == id);

            if (utilisateurModel != null)
            {
                if (utilisateurModel.Adherent != null)
                    throw new Exception("Impossible : L'utilisateur est un adhérent.");
                _context.Utilisateurs.Remove(utilisateurModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
