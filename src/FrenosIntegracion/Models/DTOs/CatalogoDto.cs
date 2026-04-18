namespace FrenosIntegracion.Models.DTOs
{
    public record ProductoDto(
        int Id,
        string Nombre,
        decimal Precio,
        int Stock,
        string? Categoria,
        bool Activo
    );

    public record ServicioDto(
        int Id,
        string Nombre,
        decimal Precio,
        int? DuracionMin,
        string? Categoria,
        bool Activo
    );

    public record BusquedaCatalogoResponse(
        IEnumerable<ProductoDto> Productos,
        IEnumerable<ServicioDto> Servicios
    );

}
