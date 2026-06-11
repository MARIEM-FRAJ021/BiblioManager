using BiblioManager.API.Models;

namespace BiblioManager.API.DAL
{
    public static class SeedData
    {
        public static void Initialize(BiblothequeDbContext context)
        {
            if (context.Utilisateurs.Any())
                return;
            var hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var admin = new Utilisateur
            {
                Nom = "Admin",
                Prenom = "System",
                Email = "mariem.fraj99@gmail.com",
                MotDePasse = hash,
                RoleUtilisateur = RoleUtilisateurEnum.Admin
            };
            context.Utilisateurs.Add(admin);
            context.SaveChanges();
        }
    }
}
