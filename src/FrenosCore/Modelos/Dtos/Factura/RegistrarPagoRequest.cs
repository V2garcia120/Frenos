using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Factura
{
    public record RegistrarPagoRequest(
        [Required] int TurnoId,
        [Required] string Metodo,
        [Required] decimal Monto
    );
}
