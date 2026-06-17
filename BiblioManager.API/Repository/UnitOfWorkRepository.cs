using BiblioManager.API.DAL;
using BiblioManager.API.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BiblioManager.API.Repository
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly BiblothequeDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWorkRepository(BiblothequeDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
