using FrenosIntegracion.DTOs;
using FrenosIntegracion.Models.DTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
        Task<object> AbrirTurnoAsync(AbrirTurnoRequest request);
        Task<object> CerrarTurnoAsync(CerrarTurnoRequest request);
        Task<object> RegistrarMovimientoEfectivoAsync(MovimientoEfectivoRequest request);
        Task<object> PagarFacturaAsync(PagoFacturaRequest request, string token);
        Task<object> RegistrarAbonoAsync(AbonoCxCRequest request);
        Task<CobroResponse> ProcesarCobroAsync(CobroRequest request, string token);
        Task<bool> EstaDisponibleAsync();
        Task<IEnumerable<object>> ObtenerFacturasPorClienteAsync(int clienteId);
        Task<object> ObtenerFacturasPendientesAsync(string token, string? numeroFactura, string? placa);
        Task<IEnumerable<object>> ObtenerVehiculosClienteAsync();
        Task<object> RegistrarVehiculoAsync(object vehiculo);
        Task<IEnumerable<object>> ObtenerHistorialOrdenesAsync();
        Task<bool> RegistrarClienteAsync(ClienteRegistroDto cliente);
    }
}
