namespace FrenosCore.Modelos.Dtos.Servicio
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
