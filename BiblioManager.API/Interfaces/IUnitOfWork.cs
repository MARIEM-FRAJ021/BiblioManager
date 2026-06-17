using Microsoft.EntityFrameworkCore.Storage;

namespace BiblioManager.API.Interfaces
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

    }
}
