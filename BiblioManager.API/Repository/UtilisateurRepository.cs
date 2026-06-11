using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

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
            return await _context.Utilisateurs.Include(u => u.Adherent).ToListAsync();
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            return await _context.Utilisateurs.Include(x => x.Adherent).FirstOrDefaultAsync(x => x.IdUtilisateur == id);
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur user)
        {
            var users = _context.Utilisateurs;
            var emailExiste = await users.AnyAsync(u=> u.Email == user.Email);
            if (emailExiste)
                throw new Exception("Cet email existe déjà.");
            if (user.RoleUtilisateur is RoleUtilisateurEnum.Admin)
                throw new Exception("Interdit");
            if (user.RoleUtilisateur is RoleUtilisateurEnum.Adherent)
                throw new Exception("Interdit");
            await users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Utilisateur?> UpdateAsync(int id, Utilisateur user)
        {
            var users = _context.Utilisateurs;
            var userModel = await users.FirstOrDefaultAsync(x => x.IdUtilisateur == id);
            if (userModel == null)
                return null;
            var emailExiste = await users.AnyAsync(u => u.Email == user.Email);
            if (emailExiste)
                throw new Exception("Cet email existe déjà.");
            if (user.RoleUtilisateur is RoleUtilisateurEnum.Admin)
                throw new Exception("Interdit");
            if (user.RoleUtilisateur is RoleUtilisateurEnum.Adherent)
                throw new Exception("Interdit");
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
                if (utilisateurModel.RoleUtilisateur == RoleUtilisateurEnum.Admin || utilisateurModel.RoleUtilisateur == RoleUtilisateurEnum.Adherent)
                    throw new Exception(" Interdit de supprimer un adhérent / admin");
                _context.Utilisateurs.Remove(utilisateurModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
