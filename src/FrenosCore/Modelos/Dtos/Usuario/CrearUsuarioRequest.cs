namespace FrenosCore.Modelos.Dtos.Usuario
{
    public record CrearUsuarioRequest(
        string Nombre,
        string Email,
        string Password,
        int RolId,
        bool Activo
    );
}
