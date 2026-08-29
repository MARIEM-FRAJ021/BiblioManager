using BiblioManager.API.DAL;
using Microsoft.EntityFrameworkCore;

namespace BiblioManager.API.BackgroundServices
{
    public class MaintenanceBatchService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MaintenanceBatchService> _logger;
        public MaintenanceBatchService(IServiceScopeFactory scopeFactory, ILogger<MaintenanceBatchService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<BiblothequeDbContext>();
                    var now = DateTime.UtcNow;

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
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Echec du job de maintenance");
                }
                try
                {
                    await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
