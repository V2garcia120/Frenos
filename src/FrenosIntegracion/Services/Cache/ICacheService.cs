using FrenosIntegracion.Models.DTOs;

namespace FrenosIntegracion.Services.Cache
{
    public interface ICacheService
    {
        Task<IEnumerable<ProductoDto>> ObtenerProductosAsync();
        Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync();
        Task RefrescarAsync();
        DateTime UltimaActualizacion { get; }
    }
}
