namespace FrenosCore.Modelos.Dtos
{
    public record CrearUsuarioRequest(
        string Nombre,
        string Email,
        string Password,
        int RolId,
        bool Activo
    );
}
