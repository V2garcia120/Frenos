using TallerCaja.Models.DTOs;

namespace TallerCaja.Services
{
    // ── Servicio de sincronización ────────────────────────────────────────────
    public interface ISyncService
    {
        Task<SyncResponse> SincronizarPendientesAsync();
    }
}
