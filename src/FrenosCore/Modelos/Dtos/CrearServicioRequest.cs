namespace FrenosCore.Modelos.Dtos
{
    public record CrearServicioRequest(
        string Nombre,
        string Descripcion,
        decimal Precio,
        int DuracionMinutos,
        string Categoria,
        bool Activo
    );
}
