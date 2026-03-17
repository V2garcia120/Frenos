using FrenosCore.Modelos.Dtos;

namespace FrenosCore.Servicios
{
    public interface IServiciciosService
    {
        Task<ServicioResponse> CrearAsync(CrearServicioRequest request);
        Task<IEnumerable<ServicioResponse>> ListarAsync();

        Task<IEnumerable<ServicioResponse>> BuscarAsync(string? termino);
        Task<ServicioResponse> ObtenerPorIdAsync(int id);
        Task<ServicioResponse> ActualizarAsync(int id, ActualizarServicioRequest request);
        Task<bool> EliminarAsync(int id);
        

    }
}
