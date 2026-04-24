namespace FrenosCore.Modelos.Dtos.Factura
{
    public record CobroDirectoResponse(
        int FacturaId,
        string NumeroFactura,
        decimal Total,
        decimal Cambio,
        string Estado
    );
}
