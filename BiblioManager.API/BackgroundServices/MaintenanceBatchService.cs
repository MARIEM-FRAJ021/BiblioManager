using BiblioManager.API.DAL;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.BackgroundServices
{
    public class MaintenanceBatchService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public MaintenanceBatchService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<BiblothequeDbContext>();
                var now = DateTime.Now;

                var adherents = await context.Adherents
                                        .Where(a => a.Actif && a.DateFin < now)
                                        .ExecuteUpdateAsync(
                                        s => s.SetProperty(a => a.Actif, false),
                                        cancellationToken);
                var emprunts = await context.Emprunts
                                        .Where(e => e.DateRetourEffective == null && e.DateRetourPrevue < now)
                                        .ExecuteUpdateAsync(
                                        s => s.SetProperty(e => e.Statut, Models.StatutEmprunt.EnRetard),
                                        cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);

            }
        }
    }
}
