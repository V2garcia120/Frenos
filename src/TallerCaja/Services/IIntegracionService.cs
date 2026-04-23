using TallerCaja.Models.DTOs;

namespace TallerCaja.Services
{
    // ── Interfaz del servicio de Integración ──────────────────────────────────
    // PUNTO DE CONEXIÓN: Cuando Integración entregue sus APIs reales,
    // solo modifica IntegracionService. Esta interfaz no cambia.
    public interface IIntegracionService
    {
        Task<HealthCheckDto?> HealthCheckAsync();
        Task<LoginCajeroResponse?> LoginCajeroAsync(string email, string password);
        Task<List<ProductoDto>> ObtenerProductosAsync(string? categoria = null);
        Task<List<ServicioDto>> ObtenerServiciosAsync();
        Task<BusquedaCatalogoDto?> BuscarCatalogoAsync(string q);
        Task<List<ClienteDto>> BuscarClientesAsync(string q);
        Task<ClienteDto?> ObtenerClienteAnonimoAsync();
        Task<AbrirTurnoResponse?> AbrirTurnoAsync(AbrirTurnoRequest request);
        Task<CerrarTurnoResponse?> CerrarTurnoAsync(CerrarTurnoRequest request);
        Task<CobroResponse?> ProcesarCobroAsync(CobroRequest request);
        Task<FacturaPendienteDto?> BuscarFacturaPendienteAsync(string? placa = null, string? numero = null);
        Task<PagoFacturaResponse?> PagarFacturaAsync(int facturaId, PagoFacturaRequest request);
        Task<AbonoResponse?> RegistrarAbonoAsync(int cxcId, AbonoRequest request);
        Task<SyncResponse?> SincronizarAsync(SyncRequest request);
    }
}
