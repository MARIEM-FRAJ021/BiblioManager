using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Services
{
    public class AdherentService : IAdherentService
    {
        private readonly BiblothequeDbContext _context;

        public AdherentService(BiblothequeDbContext context)
        {
            _context = context;
        }
        public async Task DevenirAdherent(int utilisateurId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var data = await _context.Utilisateurs
               .Where(u => u.IdUtilisateur == utilisateurId)
               .Select(
               u => new
               {
                   Utilisateur = u,
                   EstAdherent = _context.Adherents.Any(a => a.IdUtilisateur == utilisateurId),
                   PaiementValide = _context.Paiements.Where(p => p.IdUtilisateur == utilisateurId && p.Statut == PaiementStatutEnum.Valide && p.Type == TypePaiement.Abonnement).OrderByDescending(p => p.DatePaiement).FirstOrDefault()
               }
               ).FirstOrDefaultAsync();
                if (data == null)
                    throw new Exception("Utilisateur introuvable");
                if (data.EstAdherent)
                    throw new Exception("Utilisateur est déjà adhérent");
                if (data.PaiementValide == null)
                    throw new Exception("paiement requis");

                data.Utilisateur.RoleUtilisateur = RoleUtilisateurEnum.Adherent;

                var adherent = new Adherent
                {
                    IdUtilisateur = utilisateurId,
                    Nom = data.Utilisateur.Nom,
                    Prenom = data.Utilisateur.Prenom,
                    Email = data.Utilisateur.Email,
                    Actif = true,
                    DateDebut = DateTime.Now,
                    DateFin = DateTime.Now.AddYears(1),
                    Penalite = 0
                };

                _context.Adherents.Add(adherent);
                data.PaiementValide.Statut = PaiementStatutEnum.Consomme;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> AdherentEstActif(int idAdherent)
        {
            var adherent = await _context.Adherents.FirstOrDefaultAsync(a => a.IdAdherent == idAdherent);
            if (adherent == null)
                return false;
            return adherent.Actif && adherent.DateFin >= DateTime.Now;
        }
        public async Task RenouvelerAbonnement(int idAdherent)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var adherent = await _context.Adherents.FirstOrDefaultAsync(a => a.IdAdherent == idAdherent)
                              ?? throw new KeyNotFoundException();
                var paiement = await _context.Paiements.Where(p => p.Statut == PaiementStatutEnum.Valide && p.Type == TypePaiement.Abonnement && p.IdUtilisateur == adherent.IdUtilisateur).OrderByDescending(p => p.DatePaiement).FirstOrDefaultAsync()
                ?? throw new InvalidOperationException("Aucun paiement pour cet adhérent.");
                adherent.DateDebut = DateTime.Now;
                adherent.DateFin = DateTime.Now.AddYears(1);
                adherent.Actif = true;
                paiement.Statut = PaiementStatutEnum.Consomme;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> DesactiverAdhesion(int idAdherent)
        {
            var adherent = await _context.Adherents.FirstOrDefaultAsync(a => a.IdAdherent == idAdherent) ?? throw new KeyNotFoundException();
            if (adherent.DateFin < DateTime.Now)
                adherent.Actif = false;

            await _context.SaveChangesAsync();
            return adherent.Actif;
        }
    }
}
