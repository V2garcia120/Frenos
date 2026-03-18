namespace FrenosCore.Modelos.Dtos.Producto
{
    public record ActualizarProductoRequest(
        string? Nombre,
        string? Descripcion,
        decimal? Precio,
        decimal? Costo,
        int? Stock,
        int? StockMinimo,
        string? Categoria,
        bool? Activo
    );
}
