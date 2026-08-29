using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;

namespace BiblioManager.API.Services
{
    public class EmpruntService : IEmpruntService
    {
        private readonly IEmpruntRepository _empruntRepository;
        private readonly IAdherentService _adherentService;
        private readonly ILivreRepository _livreRepository;

        public EmpruntService(IEmpruntRepository empruntRepository, IAdherentService adherentService, ILivreRepository livreRepository)
        {
            _empruntRepository = empruntRepository;
            _adherentService = adherentService;
            _livreRepository = livreRepository;
        }

        public async Task EmprunterLivre(int idAdherent, int idLivre)
        {
            await _adherentService.VerifierAdherentActif(idAdherent);
            var livre = await _livreRepository.GetByIdAsync(idLivre) ?? throw new KeyNotFoundException("Livre introuvable");
            if (livre.QuantiteDisponible <= 0)
                throw new InvalidOperationException("Livre indisponible");

            var emprunt = new Emprunt
            {
                IdAdherent = idAdherent,
                IdLivre = idLivre,
                DateEmprunt = DateTime.UtcNow,
                DateRetourPrevue = DateTime.UtcNow.AddDays(14)

            };

            await _empruntRepository.AjouterAsync(emprunt);
            livre.QuantiteDisponible--;
            await _livreRepository.SaveChangesAsync();
        }

        public async Task RetournerLivre(int idAdherent,int idEmprunt)
        {
            var emprunt = await _empruntRepository.GetByIdAsync(idAdherent,idEmprunt)
            ?? throw new KeyNotFoundException("Emprunt introuvable");
            if (emprunt.DateRetourEffective != null)
                throw new InvalidOperationException("Livre déjà retourné");
            emprunt.DateRetourEffective = DateTime.UtcNow;
            emprunt.Livre!.QuantiteDisponible++;
            emprunt.Statut = StatutEmprunt.Retourne;
            if (emprunt.DateRetourEffective > emprunt.DateRetourPrevue)
            {
                var joursRetard = (int)Math.Ceiling(
            (emprunt.DateRetourEffective.Value - emprunt.DateRetourPrevue).TotalDays);
                emprunt.Adherent!.Penalite += joursRetard;
            }

            await _livreRepository.SaveChangesAsync();
        }
    }
}
