using FrenosIntegracion.DTOs;
using FrenosIntegracion.Models.DTOs;

namespace FrenosIntegracion.Services.Core
{
    public interface ICoreService
    {
        // Autenticación
        Task<object> AutenticarClienteAsync(LoginRequest request);
        Task<object> AutenticarEmpleadoAsync(LoginRequest request);
        Task<object> AutenticarCajeroAsync(LoginRequest request);
        // Catálogo
        Task<IEnumerable<ProductoDto>> ObtenerProductosAsync();
        Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync();

        // Órdenes
        Task<OrdenWebResponse> CrearOrdenAsync(CrearOrdenWebRequest request, string token);
        Task<EstadoOrdenResponse> ObtenerEstadoOrdenAsync(int ordenId, string token);

        // Caja
        Task<object> AbrirTurnoAsync(int cajeroId, decimal montoInicial);
        Task<object> CerrarTurnoAsync(int turnoId, decimal efectivoContado);
        Task<object> RegistrarMovimientoEfectivoAsync(int turnoId, decimal monto, string motivo, string tipo);
        Task<object> PagarFacturaAsync(int facturaId, int turnoId, string metodo, decimal monto);
        Task<object> RegistrarAbonoAsync(int cxcId, int turnoId, decimal monto, string metodo);
        Task<CobroResponse> ProcesarCobroAsync(CobroRequest request, string token);
        Task<bool> EstaDisponibleAsync();
        Task<IEnumerable<object>> ObtenerFacturasPorClienteAsync(int clienteId);

        Task<IEnumerable<object>> ObtenerVehiculosClienteAsync();
        Task<object> RegistrarVehiculoAsync(object vehiculo);
        Task<IEnumerable<object>> ObtenerHistorialOrdenesAsync();
        Task<bool> RegistrarClienteAsync(ClienteRegistroDto cliente);
    }
}
