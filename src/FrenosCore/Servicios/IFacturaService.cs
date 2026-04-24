using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Factura;


namespace FrenosCore.Servicios
{
    public interface IFacturaService
    {
        Task<PaginadoResponse<FacturaResponse>> ListarAsync(
        int pagina, int tam, string? estado, string? numero, DateTime? fecha, string? tipoOrigen);


        Task<FacturaResponse> ObtenerPorIdAsync(int id);
        Task<FacturaPendienteDto> ObtenerFacturaPendientesAsync(string? placa = null, string? numeroFactura = null);

        Task<FacturaResponse> GenerarDesdeOrdenAsync(int ordenId, int emisorId, string? metodoPago);


        Task<FacturaResponse> RegistrarPagoAsync(int id, RegistrarPagoRequest request);


        Task AnularAsync(int id);
    }
}
