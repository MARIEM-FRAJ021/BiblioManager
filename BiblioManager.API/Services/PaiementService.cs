using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;

namespace BiblioManager.API.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly IPaimentRepository _paiementRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;
        public PaiementService(IPaimentRepository paimentRepository, IUtilisateurRepository utilisateurRepository)
        {
            _paiementRepository = paimentRepository;
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task TraiterPaiementCarte(string stripeSessionId, bool paiementReussi)
        {
            var paiement = await _paiementRepository.GetPaiementToTreat(stripeSessionId);
            if (paiement == null)
                throw new KeyNotFoundException("Paiement introuvable");
            if (paiement.Statut != PaiementStatutEnum.EnAttente)
                throw new InvalidOperationException("Paiement déjà traité");
            if (paiementReussi)
            {
                paiement.Statut = Models.PaiementStatutEnum.Valide;
                if (paiement.Type == TypePaiement.Penalite)
                {
                    if (paiement.Utilisateur?.Adherent == null)
                        throw new KeyNotFoundException("Aucun adhérent associé");
                    paiement.Utilisateur.Adherent.Penalite = 0;
                }
            }
            else
            {
                paiement.Statut = Models.PaiementStatutEnum.Refuse;
            }
            await _paiementRepository.SaveChangesAsync();
        }
        public async Task<Paiement> InitierPaiementCarte(Paiement createpaiement)
        {
            var utilisateur = await _utilisateurRepository.GetByIdAsync(createpaiement.IdUtilisateur);
            if (utilisateur == null)
                throw new KeyNotFoundException("utilisateur introuvable.");
            if (utilisateur.Adherent?.DateFin >= DateTime.UtcNow && createpaiement.Type == TypePaiement.Abonnement)
                throw new KeyNotFoundException("Adhérent actif.");
            if (createpaiement.Type == TypePaiement.Penalite && utilisateur.Adherent == null)
                throw new InvalidOperationException("Utilisateur non adhérent. Pénalité impossible.");
            if (utilisateur.Adherent?.Penalite <= 0 && createpaiement.Type == TypePaiement.Penalite)
                throw new InvalidOperationException("Aucune pénalité à régler.");
            var paiement = new Paiement
            {
                IdUtilisateur = createpaiement.IdUtilisateur,
                Montant = createpaiement.Type == TypePaiement.Abonnement ? Constantes.Constantes.PrixAnnuel : utilisateur.Adherent!.Penalite,
                Mode = ModePaiementEnum.Carte,
                Statut = PaiementStatutEnum.EnAttente,
                DatePaiement = DateTime.UtcNow,
                StripeSessionId = createpaiement.StripeSessionId,
                Reference = Guid.NewGuid().ToString(),
                Type = createpaiement.Type
            };
            await _paiementRepository.AddAsync(paiement);
            await _paiementRepository.SaveChangesAsync();
            return paiement;
        }
    }
}
