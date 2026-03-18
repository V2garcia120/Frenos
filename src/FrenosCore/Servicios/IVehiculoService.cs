using FrenosCore.Modelos.Dtos.Vehiculo;

namespace FrenosCore.Servicios
{
    public interface IVehiculoService
    {
        Task<VehiculoResponse> RegistrarAsync(RegistrarVehiculoRequest request);
        Task<IEnumerable<VehiculoResponse>> ListarPorClienteAsync(int clienteId);
        Task<VehiculoResponse> ActualizarAsync(int id, ActualizarVehiculoRequest request);
        Task EliminarAsync(int id);
    }
}
