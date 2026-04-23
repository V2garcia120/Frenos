namespace TallerCaja.Services
{
    // ── Servicio de sincronización ────────────────────────────────────────────
    public interface ISyncService
    {
        Task SincronizarPendientesAsync();
    }
}
