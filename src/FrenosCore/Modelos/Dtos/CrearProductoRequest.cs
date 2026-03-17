namespace FrenosCore.Modelos.Dtos
{
    public record CrearProductoRequest(
        string Nombre,
        string Descripcion,
        decimal Precio,
        decimal Costo,
        int Stock,
        int StockMinimo,
        string Categoria,
        bool Activo

    );
}
