namespace FrenosCore.Modelos.Dtos.Producto
{
    public record ProductoResponse(
        int Id,
        string Nombre,
        string Descripcion,
        decimal Precio,
        decimal Costo,
        int Stock,
        int StockMinimo,
        string Categoria,
        bool Activo,
        DateTime CreadoEn
    );
    public record ProductoDto(
                int Id,
        string Nombre,
        decimal Precio,
        int Stock,
        string? Categoria,
        bool Activo
        );
}
