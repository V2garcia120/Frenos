namespace FrenosCore.Modelos.Dtos.Cotizacion
{
    public record CrearCotizacionRequest(
        int ClienteId,
        int VehiculoId,
        IEnumerable<CrearCotizacionItemRequest> Detalles
    );
    public record CrearCotizacionItemRequest(
        string Tipo,
        string Descripcion,
        int Cantidad,
        decimal PrecioUnitario
    );
}
