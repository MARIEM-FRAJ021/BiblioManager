using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Stripe.Checkout;

namespace BiblioManager.API.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly IPaimentRepository _paiementRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly IAdherentService _adherentService;
        public PaiementService(IPaimentRepository paimentRepository, IUtilisateurRepository utilisateurRepository, IAdherentService adherentService)
        {
            _paiementRepository = paimentRepository;
            _utilisateurRepository = utilisateurRepository;
            _adherentService = adherentService;
        }

        public async Task TraiterPaiementCarte(Session session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (session.PaymentStatus != "paid")
                return;
            var paiement = await _paiementRepository.GetPaiementToTreat(session.Id);
            if (paiement == null)
                throw new KeyNotFoundException("Paiement introuvable");
            if (paiement.Utilisateur == null)
                throw new KeyNotFoundException("utilisateur introuvable");
            if (paiement.Statut != PaiementStatutEnum.EnAttente)
                throw new InvalidOperationException("Paiement déjà traité");
            var montantStripe = session.AmountTotal ?? 0;
            var montantAttendu = (long)(paiement.Montant * 100);
            if (montantAttendu != montantStripe)
                throw new InvalidOperationException("Le montant stripe ne correspond pas au montant attendu.");
            paiement.Statut = PaiementStatutEnum.Valide;
            paiement.DatePaiement = DateTime.UtcNow;
            var isAdherent = paiement.Utilisateur?.Adherent != null;

            if (!isAdherent)
            {
                await _adherentService.DevenirAdherent(paiement);
                return;
            }

            if (paiement.Type == TypePaiement.Penalite)
            {
                paiement.Utilisateur!.Adherent!.Penalite = 0;
            }
            else if (paiement.Type == TypePaiement.Abonnement)
            {
                await _adherentService.RenouvelerAbonnement(paiement);
            }

            await _paiementRepository.SaveChangesAsync();
        }
        public async Task<string> InitierPaiementCarte(TypePaiement type, int idUtilisateur)
        {
            var utilisateur = await _utilisateurRepository.GetByIdAsync(idUtilisateur);
            if (utilisateur == null)
                throw new KeyNotFoundException("utilisateur introuvable.");
            if (utilisateur.Adherent?.Actif == true && type == TypePaiement.Abonnement)
                throw new KeyNotFoundException("Adhérent actif.");
            if (type == TypePaiement.Penalite && utilisateur.Adherent == null)
                throw new InvalidOperationException("Utilisateur non adhérent. Pénalité impossible.");
            if (utilisateur.Adherent?.Penalite <= 0 && type == TypePaiement.Penalite)
                throw new InvalidOperationException("Aucune pénalité à régler.");
            var paiement = new Paiement
            {
                IdUtilisateur = idUtilisateur,
                Montant = type == TypePaiement.Abonnement ? Constantes.Constantes.PrixAnnuel : utilisateur.Adherent!.Penalite,
                Mode = ModePaiementEnum.Carte,
                Statut = PaiementStatutEnum.EnAttente,
                DatePaiement = DateTime.UtcNow,
                Reference = Guid.NewGuid().ToString(),
                Type = type
            };
            await _paiementRepository.AddAsync(paiement);

            var sessionOptions = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = "https://localhost:4200/payment-success" +
        "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:4200/payment-cancel",

                LineItems = new List<SessionLineItemOptions>
                {
                    new() {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency="eur",
                            UnitAmount = (long)(paiement.Montant*100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                              Name= paiement.Type == TypePaiement.Abonnement ? "Abonnement BiblioManager" : "Règlement de pénalité"

                            }
                        },

                    }
                },
                Metadata = new Dictionary<string, string>()
                {
                    ["PaiementId"] = paiement.IdPaiement.ToString(),
                    ["IdUtilisateur"] = paiement.IdUtilisateur.ToString(),
                    ["TypePaiment"] = paiement.Type.ToString(),
                }
            };

            var sessionService = new SessionService();

            var session = await sessionService.CreateAsync(sessionOptions);

            paiement.StripeSessionId = session.Id;
            await _paiementRepository.SaveChangesAsync();

            return session.Url;
        }
    }
}
