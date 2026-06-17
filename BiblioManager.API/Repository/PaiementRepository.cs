using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace BiblioManager.API.Repository
{
    public class PaiementRepository : IPaimentRepository
    {
        private readonly BiblothequeDbContext _context;

        public PaiementRepository(BiblothequeDbContext context)
        {
            _context = context;
        }
        public async Task<Paiement?> GetById(int id)
        {
            return await _context.Paiements.FirstOrDefaultAsync(p => p.IdPaiement == id);
        }
        public async Task<IEnumerable<Paiement>> GetPaiementsUtilisateur(int idUtilisateur)
        {
            return await _context.Paiements
                .Where(p => p.IdUtilisateur == idUtilisateur && p.Statut != PaiementStatutEnum.EnAttente)
                .OrderByDescending(p => p.DatePaiement)
                .ToListAsync();
        }

        public async Task<Paiement?> GetDernierPaiementValide(int idUtilisateur)
        {
            return await _context.Paiements.Where(p => p.IdUtilisateur == idUtilisateur && p.Statut == PaiementStatutEnum.Valide && p.Type == TypePaiement.Abonnement).OrderByDescending(p => p.DatePaiement).FirstOrDefaultAsync();
        }

        public async Task<Paiement?> GetPaiementToTreat(string stripeSessionId)
        {
            return await _context.Paiements
                .Include(p => p.Utilisateur)
                .ThenInclude(u => u.Adherent)
                .FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId);
        }

        public async Task AddAsync(Paiement paiement)
        {
            await _context.Paiements.AddAsync(paiement);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
