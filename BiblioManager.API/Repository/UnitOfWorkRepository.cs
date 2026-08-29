using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BiblioManager.API.Repository
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly BiblothequeDbContext _context;

        public UnitOfWorkRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
