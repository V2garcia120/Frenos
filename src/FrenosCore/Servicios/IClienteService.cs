using FrenosCore.Modelos.Dtos;

namespace FrenosCore.Servicios
{
    public interface IClienteService
    {
        Task<PaginadoResponse<ClienteResponse>> ListarAsync(
            int pagina, int tam, string? busqueda);

        Task<ClienteDetalleResponse> ObtenerPorIdAsync(int id);

        Task<IEnumerable<ClienteResponse>> BuscarAsync(string termino);

        Task<ClienteResponse> ObtenerAnonimoAsync();

        Task<ClienteResponse> CrearAsync(CrearClienteRequest request);
        
        Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest request);

        Task EliminarAsync(int id);

    }
}
