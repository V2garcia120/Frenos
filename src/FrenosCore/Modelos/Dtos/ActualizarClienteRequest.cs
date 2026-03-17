using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos
{
    public record ActualizarClienteRequest(
        [MaxLength(200)] string? Nombre,
        [MaxLength(20)] string? Cedula,
        [MaxLength(20)] string? Telefono,
        [EmailAddress, MaxLength(200)] string? Email,
        [MaxLength(300)] string? Direccion
    );
}
