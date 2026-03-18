namespace FrenosCore.Modelos.Dtos.Cotizacion
{
    public record ActualizarCotizacionRequest(
        );
    public record ActualizarCotizacionItemRequest(

        int Cantidad,
        int PrecioUnitario

        );
}
