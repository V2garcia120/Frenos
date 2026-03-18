namespace FrenosCore.Modelos.Dtos.Usuario
{
    public record UsuarioResponse(
        int Id,
        string Nombre,
        string Email,
        int RolId,
        string Rol,
        bool Activo,
        DateTime FechaCreacion,
        DateTime? UltimoLogin
    );
}
