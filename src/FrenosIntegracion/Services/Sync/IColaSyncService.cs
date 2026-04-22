using FrenosIntegracion.Models.DTOs;

namespace FrenosIntegracion.Services.Sync
{
    public interface IColaSyncService
    {
        Task<bool> EstaDisponibleAsync();

        // Autenticación (Pág. 7)
        Task<object> AutenticarClienteAsync(LoginRequest request);
        Task<object> AutenticarEmpleadoAsync(LoginRequest request);

        // Turnos y Caja (Pág. 10-11)
        Task<object> AbrirTurnoAsync(int cajeroId, decimal montoInicial);
        Task<object> RegistrarMovimientoEfectivoAsync(int turnoId, decimal monto, string motivo, string tipo);

        // Facturación y Pagos (Pág. 12-13)
        Task<object> PagarFacturaAsync(int facturaId, int turnoId, string metodo, decimal monto);
        Task<object> RegistrarAbonoAsync(int cxcId, int turnoId, decimal monto, string metodo);

        Task<object> ProcesarLoteOfflineAsync(SyncRequest request);
        Task EncolarOperacionAsync(string canal, string tipo, object payload);
        Task ProcesarPendientesAsync();
    }
}