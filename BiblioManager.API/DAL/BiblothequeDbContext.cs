using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.DAL
{
    public class BiblothequeDbContext : DbContext
    {
        public BiblothequeDbContext(DbContextOptions<BiblothequeDbContext> options) : base(options)
        {
        }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Auteur> Auteurs { get; set; }

        public DbSet<Emprunt> Emprunts { get; set; }

        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigUtilisateur(modelBuilder);
            ConfigAdherent(modelBuilder);
            ConfigLivre(modelBuilder);
            ConfigCategorie(modelBuilder);
            ConfigAuteur(modelBuilder);
            ConfigEmprunt(modelBuilder);
            ConfigPaiement(modelBuilder);
        }
        

        private void ConfigUtilisateur (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Adherent)
                .WithOne(a=> a.Utilisateur)
                .HasForeignKey<Adherent>(a=>a.IdUtilisateur)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        private void ConfigAdherent (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adherent>()
                .HasMany(a => a.Emprunts)
                .WithOne(e=>e.Adherent)
                .HasForeignKey(e=> e.IdAdherent)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void ConfigLivre (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livre>()
                .HasMany(l=>l.Emprunts)
                .WithOne(e=>  e.Livre)
                .HasForeignKey (e=> e.IdLivre)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Livre>()
                .Property(l => l.ISBN)
                .HasMaxLength(20);
        }

        private void ConfigCategorie(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livre>()
                .HasOne(l =>  l.Categorie)
                .WithMany(c=> c.Livres)
                .HasForeignKey(l=> l.IdCategorie)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigAuteur(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Livre>()
                .HasOne (l => l.Auteur)
                .WithMany(a=>a.Livres)
                .HasForeignKey(l=>l.AuteurId)
                        .OnDelete(DeleteBehavior.Restrict);

        }

        private void ConfigEmprunt(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Emprunt>()
                    .Property(e => e.Statut)
                    .HasConversion<string>();
            modelBuilder.Entity<Emprunt>()
                .HasIndex(e => new { e.IdLivre, e.IdAdherent, e.DateEmprunt });
        
        }

        private void ConfigPaiement (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paiement>()
            .HasOne(p =>p.Adherent)
            .WithMany (a=>a.Paiements)
            .HasForeignKey(p=>p.IdAdherent)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Paiement>()
                .Property(p=> p.Montant)
                .HasColumnType("decimal(10,2)");
        }
    }
}

