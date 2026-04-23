using FrenosCore.Modelos.Dtos.Vehiculo;

namespace FrenosCore.Servicios
{
    public interface IVehiculoService
    {
        Task<VehiculoResponse> RegistrarAsync(RegistrarVehiculoRequest request);
        Task<IEnumerable<VehiculoResponse>> ListarPorClienteAsync(int clienteId, bool soloactivos = true);
        Task<IReadOnlyList<HistorialReparacionResponse>> ListarHistorialReparacionesAsync(int vehiculoId);
        Task<VehiculoResponse> ActualizarAsync(int id, ActualizarVehiculoRequest request);
        Task DesactivarAsync(int id);
    }
}
