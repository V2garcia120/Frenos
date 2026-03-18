using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Producto;

namespace FrenosCore.Servicios
{
    public interface IProductoService
    {
        public Task<ProductoResponse> CrearProductoAsync(CrearProductoRequest request);
        public Task<ProductoResponse> ObtenerProductoPorIdAsync(int id);
        public Task <IEnumerable<ProductoResponse>> ListarTodosAsync(string? categoria);
        public Task<IEnumerable<ProductoResponse>> BuscarAsync(string? termino);
        public Task<ProductoResponse> ActualizarProductoAsync(int id, ActualizarProductoRequest request);
        public Task EliminarProductoAsync(int id);


    }
}
