using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Orden;  

namespace FrenosCore.Servicios
{
    public interface IOrdenService
    {
        Task<PaginadoResponse<OrdenResponse>> ListarAsync(
            int pagina, int tam, string? estado, string? prioridad);
        
        Task<OrdenDetalleResponse> ObtenerPorIdAsync(int id);
        Task<OrdenResponse> CrearAsync(CrearOrdenRequest request);
        Task<OrdenResponse> CambiarEstadoAsync(int id, CambiarEstadoOrdenRequest request);
        Task<CerrarOrdenResponse> CerrarAsync(int id, CerrarOrdenRequest request);
    }
}
