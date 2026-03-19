using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Factura;


namespace FrenosCore.Servicios
{
    public interface IFacturaService
    {
        Task<PaginadoResponse<FacturaResponse>> ListarAsync(
        int pagina, int tam, string? estado);


        Task<FacturaResponse> ObtenerPorIdAsync(int id);


        Task<FacturaResponse> GenerarDesdeOrdenAsync(int ordenId, int emisorId);


        Task<FacturaResponse> RegistrarPagoAsync(int id, RegistrarPagoRequest request);


        Task AnularAsync(int id);
    }
}
