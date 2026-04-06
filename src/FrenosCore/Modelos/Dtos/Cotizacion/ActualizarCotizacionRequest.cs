namespace FrenosCore.Modelos.Dtos.Cotizacion
{
    public record ActualizarCotizacionRequest(
        string? Notas,
        DateTime? ValidaHasta,
        IEnumerable<ActualizarCotizacionDetalleRequest>? Detalles
    );

    public record ActualizarCotizacionDetalleRequest(
        int? Id,
        string Tipo,
        string Descripcion,
        int Cantidad,
        decimal PrecioUnitario,
        int? ItemId
    );
}
