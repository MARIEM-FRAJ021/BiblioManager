using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;

namespace BiblioManager.API.Services
{
    public class AdherentService : IAdherentService
    {
        private readonly IAdherentRepository _adherentRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IPaimentRepository _paiementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdherentService(IAdherentRepository adherentRepository, IUtilisateurRepository utilisateurRepository, IPaimentRepository paiementRepository, IUnitOfWork unitOfWork)
        {
            _adherentRepository = adherentRepository;
            _utilisateurRepository = utilisateurRepository;
            _paiementRepository = paiementRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task DevenirAdherent(int utilisateurId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var utilisateur = await _utilisateurRepository.GetByIdAsync(utilisateurId);

                if (utilisateur == null)
                    throw new KeyNotFoundException("Utilisateur introuvable.");

                var estAdherent =
                    await _adherentRepository.UserIsAdherent(utilisateurId);

                var paiementValide =
                    await _paiementRepository.GetDernierPaiementValide(utilisateurId);

                var data = new
                {
                    Utilisateur = utilisateur,
                    EstAdherent = estAdherent,
                    PaiementValide = paiementValide
                };
                if (data == null)
                    throw new KeyNotFoundException("Utilisateur introuvable");
                if (data.EstAdherent)
                    throw new InvalidOperationException("Utilisateur est déjà adhérent");
                if (data.PaiementValide == null)
                    throw new InvalidOperationException("paiement requis");

                data.Utilisateur.RoleUtilisateur = RoleUtilisateurEnum.Adherent;

                var adherent = new Adherent
                {
                    IdUtilisateur = utilisateurId,
                    Nom = data.Utilisateur.Nom,
                    Prenom = data.Utilisateur.Prenom,
                    Email = data.Utilisateur.Email,
                    Actif = true,
                    DateDebut = DateTime.UtcNow,
                    DateFin = DateTime.UtcNow.AddYears(1),
                    Penalite = 0
                };

                await _adherentRepository.AddAsync(adherent);

                data.PaiementValide.Statut = PaiementStatutEnum.Consomme;

                await _adherentRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task VerifierAdherentActif(int idAdherent)
        {
            var statut = await _adherentRepository.GetStatutAdherent(idAdherent);

            switch (statut)
            {
                case StatutAdherentEnum.NonAdherent:
                    throw new InvalidOperationException("Utilisateur non adhérent.");

                case StatutAdherentEnum.Expire:
                    throw new InvalidOperationException("Adhésion expirée.");

                case StatutAdherentEnum.Desactive:
                    throw new InvalidOperationException("Adhésion désactivée.");
                
                case StatutAdherentEnum.PenaliteNonReglee:
                    throw new InvalidOperationException("Pénalité non règlée.");

                case StatutAdherentEnum.Actif:
                    return;
                default:
                    throw new InvalidOperationException("Statut inconnu.");
            }
        }
        public async Task RenouvelerAbonnement(int idAdherent)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var adherent = await _adherentRepository.GetAdherentById(idAdherent)
                              ?? throw new KeyNotFoundException();
                bool adherentIsActive = adherent.Actif;
                if (adherentIsActive)
                    throw new InvalidOperationException("L'adhésion est déjà active.");
                var paiement = await _paiementRepository.GetDernierPaiementValide(adherent.IdUtilisateur)
                ?? throw new InvalidOperationException("Aucun paiement pour cet adhérent.");
                adherent.DateDebut = DateTime.UtcNow;
                adherent.DateFin = DateTime.UtcNow.AddYears(1);
                adherent.Actif = true;
                paiement.Statut = PaiementStatutEnum.Consomme;
                await _adherentRepository.SaveChangesAsync();
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
            var adherent = await _adherentRepository.GetAdherentById(idAdherent)
                ?? throw new KeyNotFoundException("Adhérent introuvable.");

            if (!adherent.Actif)
                return false;

            adherent.Actif = false;

            await _adherentRepository.SaveChangesAsync();

            return true;
        }
    }
}
