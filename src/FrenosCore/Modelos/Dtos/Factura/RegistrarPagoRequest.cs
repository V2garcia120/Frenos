using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Factura
{
    public record RegistrarPagoRequest(
        [Required] string MetodoPago,
        [Required] decimal MontoPagado
    );
}
