using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Cliente
{
    public record CrearClienteRequest(
    [Required, MaxLength(200)] string Nombre,
    [MaxLength(20)] string? Cedula,
    [MaxLength(20)] string? Telefono,
    [EmailAddress, MaxLength(200)] string? Email,
    string ? Password,
    [MaxLength(300)] string? Direccion
    );
}
