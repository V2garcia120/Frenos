namespace FrenosCore.Modelos.Dtos
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
}
