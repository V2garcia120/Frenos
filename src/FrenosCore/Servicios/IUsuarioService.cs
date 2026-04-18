using FrenosCore.Modelos.Dtos.Usuario;

namespace FrenosCore.Servicios
{
    public interface IUsuarioService
    {
        Task<IEnumerable<RolUsuarioResponse>> ListarRolesAsync();
        Task<IEnumerable<UsuarioResponse>> ListarAsync(string? busqueda);
        Task<UsuarioResponse> ObtenerPorIdAsync(int id);
        Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request);
        Task<UsuarioResponse> ActualizarAsync(int id, ActualizarUsuarioRequest request);
        Task EliminarAsync(int id);
    }
}
