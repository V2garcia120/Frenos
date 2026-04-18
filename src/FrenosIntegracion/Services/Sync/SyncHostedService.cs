using FrenosIntegracion.Services.Cache;

namespace FrenosIntegracion.Services.Sync
{
    public class SyncHostedService(
    IServiceScopeFactory scopeFactory,
    IConfiguration config,
    ILogger<SyncHostedService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalo = int.Parse(config["Sync:IntervaloSegundos"] ?? "30");
            logger.LogInformation("SyncHostedService iniciado. Intervalo: {s}s", intervalo);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    var cola = scope.ServiceProvider.GetRequiredService<IColaSyncService>();

                    await cache.RefrescarAsync();          // 1. Actualizar caché
                    await cola.ProcesarPendientesAsync();  // 2. Procesar cola
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error en SyncHostedService");
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalo), stoppingToken);
            }
        }
    }
}
