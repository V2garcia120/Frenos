namespace FrenosCore.Modelos.Dtos.Servicio
{
    public record ServicioResponse(
        int Id,
        string Nombre,
        string Descripcion,
        decimal Precio,
        int DuracionMinutos,
        string Categoria,
        bool Activo,
        DateTime CreadoEn
    );

    public record ServicioDto(
        int Id,
        string Nombre,
        decimal Precio,
        int? DuracionMin,
        string? Categoria,
        bool Activo
    );
}
