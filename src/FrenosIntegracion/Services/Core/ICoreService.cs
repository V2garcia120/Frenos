using FrenosIntegracion.Models.DTOs;

namespace FrenosIntegracion.Services.Core
{
    public interface ICoreService
    {
        // Catálogo
        Task<IEnumerable<ProductoDto>> ObtenerProductosAsync();
        Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync();

        // Órdenes
        Task<OrdenWebResponse> CrearOrdenAsync(CrearOrdenWebRequest request, string token);
        Task<EstadoOrdenResponse> ObtenerEstadoOrdenAsync(int ordenId, string token);

        // Caja
        Task<CobroResponse> ProcesarCobroAsync(CobroRequest request, string token);
        Task<bool> EstaDisponibleAsync();
    }
}
