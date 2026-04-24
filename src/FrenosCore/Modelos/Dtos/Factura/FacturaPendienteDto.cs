namespace FrenosCore.Modelos.Dtos.Factura
{
    public record FacturaPendienteDto
    (
        int Id,
         string Numero,
        string ClienteNombre,
        string VehiculoInfo,
        decimal Total,
        string Estado,
        List<ItemFacturaDto> Items
    );
    public record ItemFacturaDto(

        string Nombre,
        int Cantidad,
        decimal Precio,
        decimal Subtotal
    );
}
