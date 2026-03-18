using FrenosCore.Modelos.Dtos.Cotizacion;

namespace FrenosCore.Servicios
{
    public interface ICotizacionService
    {
        Task<CotizacionResponse> ListarAsync(int pagina, int tam);
        Task<CotizacionResponse> ObtenerPorIdAsync(int id);
        Task<CotizacionResponse> CrearAsync(CrearCotizacionRequest request);
        Task<CotizacionResponse> ActualizarAsync(int id, ActualizarCotizacionRequest request);
        Task<CotizacionResponse> GenerarDesdeDiagnosticoAsync(int diagnosticoId);
        Task<CotizacionItemResponse> ActualizarCotizacionItemAsync(int cotizacionId, int itemId, ActualizarCotizacionItemRequest request);
        Task AprobarAsync(int id);
        Task RechazarAsync(int id);
        Task EliminarAsync(int id);
    }
}
