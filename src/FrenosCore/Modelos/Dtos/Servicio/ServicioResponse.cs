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
}
