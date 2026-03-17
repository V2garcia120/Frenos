namespace FrenosCore.Modelos.Dtos
{
    public record ActualizarServicioRequest(
        string Nombre,
        string Descripcion,
        decimal Precio,
        int DuracionMinutos,
        string Categoria,
        bool Activo
    );
}
