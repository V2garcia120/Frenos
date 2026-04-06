using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;

namespace FrenosCore.Servicios
{
    public interface ICuentasPorCobrarService
    {
        Task<PaginadoResponse<CuentaPorCobrarResponse>> ListarAsync(int pagina, int tam, string? estado);
        Task<CuentaPorCobrarResponse?> ObtenerPorFacturaIdAsync(int facturaId);
        Task<CuentaPorCobrarDetalleResponse> ObtenerPorIdAsync(int id);
        Task<CuentaPorCobrarDetalleResponse> RegistrarAbonoAsync(int id, decimal monto, string metodoPago, int registradoPor);
    }
}
