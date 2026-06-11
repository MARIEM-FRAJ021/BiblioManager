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
    }
}
