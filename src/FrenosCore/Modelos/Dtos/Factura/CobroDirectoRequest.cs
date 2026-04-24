using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Factura
{
    public record CobroDirectoRequest(
        [Required] int TurnoId,
        [Required] int ClienteId,
        int? VehiculoId,
        [Required] IEnumerable<CobroDirectoItem> Items,
        [Required] string MetodoPago,
        [Required] decimal MontoPagado
    );

    public record CobroDirectoItem(
        [Required] string Tipo,
        [Required] int ItemId,
        [Required] int Cantidad,
        [Required] decimal PrecioSnapshot
    );
}
