using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Orden
{
    public record CrearOrdenRequest(
        [Required] int ClienteId,
        [Required] int VehiculoId,
                   int? TecnicoId,
                   int? CotizacionId,
        [Required] string Prioridad,
                   DateTime? FechaEntregaEstimada,
        [MaxLength(500)] string? Notas
    );
}
