namespace FrenosCore.Modelos.Dtos.Cotizacion
{
    public record CotizacionResponse(
        int Id,
        int ClienteId,
        int VehiculoId,
        decimal Subtotal,
        decimal Itbis,
        decimal Total,
        string Estado,
        DateTime ValidaHasta,
        IEnumerable<CotizacionItemResponse> Detalles
    );
    public record CotizacionItemResponse(
        string Tipo,
        string Descripcion,
        int Cantidad,
        decimal PrecioUnitario,
        decimal Subtotal
    );

}
