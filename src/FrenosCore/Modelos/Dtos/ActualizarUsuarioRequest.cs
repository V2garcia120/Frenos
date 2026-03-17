namespace FrenosCore.Modelos.Dtos
{
    public record ActualizarUsuarioRequest(
        string? Nombre,
        string? Email,
        string? Password,
        int? RolId,
        bool? Activo
    );
}
