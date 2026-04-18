namespace FrenosCore.Modelos.Dtos.Factura
{
    public record FacturaResponse(
        int Id,
        int? OrdenId,
        string TipoOrigen,
        int ClienteId,
        string ClienteNombre,
        string VehiculoInfo,
        string Numero,
        DateTime Fecha,
        decimal Subtotal,
        decimal Itbis,
        decimal Total,
        string Estado,
        string? MetodoPago,
        string EmitidaPorNombre,
        IEnumerable<FacturaItemResponse> Items
    );

    public record FacturaItemResponse(
        int Id,
        string Tipo,
        string Descripcion,
        int Cantidad,
        decimal PrecioUnitario,
        decimal Subtotal
    );
}
