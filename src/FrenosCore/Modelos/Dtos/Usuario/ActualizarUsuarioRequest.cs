namespace FrenosCore.Modelos.Dtos.Usuario
{
    public record ActualizarUsuarioRequest(
        string? Nombre,
        string? Email,
        string? Password,
        int? RolId,
        bool? Activo
    );
}
