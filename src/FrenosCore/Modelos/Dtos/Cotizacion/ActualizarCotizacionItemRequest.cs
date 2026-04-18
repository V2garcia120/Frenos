namespace FrenosCore.Modelos.Dtos.Cotizacion
{
    public record ActualizarCotizacionItemRequest(
        int Cantidad,
        decimal PrecioUnitario
    );
}
