using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly BiblothequeDbContext _context;
        public PaiementService (BiblothequeDbContext context)
        {
            _context = context;
        }


        public async Task TraiterPaiementCarte(string stripeSessionId, bool paiementReussi)
        {
            var paiement = await _context.Paiements
                .Include(p => p.Utilisateur)
                .ThenInclude(u => u.Adherent)
                .FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId);
            if (paiement == null)
                throw new Exception("Paiement introuvable");
            if (paiement.Statut != PaiementStatutEnum.EnAttente)
                throw new Exception("Paiement déjà traité");
            if (paiementReussi)
            {
                paiement.Statut = Models.PaiementStatutEnum.Valide;
                if (paiement.Type == TypePaiement.Penalite)
                {
                    if (paiement.Utilisateur?.Adherent == null)
                        throw new Exception("Aucun adhérent associé");
                    paiement.Utilisateur.Adherent.Penalite = 0;
                }
            }
            else
            {
                paiement.Statut = Models.PaiementStatutEnum.Refuse;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Paiement> InitierPaiementCarte(Paiement createpaiement)
        {
            var utilisateur = await _context.Utilisateurs.Include(u => u.Adherent)
             .FirstOrDefaultAsync(a => a.IdUtilisateur == createpaiement.IdUtilisateur);
            if (utilisateur == null)
                throw new Exception("utilisateur introuvable.");
            if (utilisateur.Adherent?.Actif == true && createpaiement.Type == TypePaiement.Abonnement)
                throw new Exception("Adhérent actif.");
            if (createpaiement.Type == TypePaiement.Penalite && utilisateur.Adherent == null)
                throw new InvalidOperationException("Utilisateur non adhérent. Pénalité impossible.");
            if (utilisateur.Adherent?.Penalite <= 0)
                throw new InvalidOperationException("Aucune pénalité à régler.");
            var paiement = new Paiement
            {
                IdUtilisateur = createpaiement.IdUtilisateur,
                Montant = createpaiement.Type == TypePaiement.Abonnement ? Constantes.Constantes.PrixAnnuel : utilisateur.Adherent.Penalite,
                Mode = ModePaiementEnum.Carte,
                Statut = PaiementStatutEnum.EnAttente,
                DatePaiement = DateTime.Now,
                StripeSessionId = createpaiement.StripeSessionId,
                Reference = Guid.NewGuid().ToString(),
                Type = createpaiement.Type
            };
            await _context.Paiements.AddAsync(paiement);
            await _context.SaveChangesAsync();
            return paiement;
        }
    }
}
