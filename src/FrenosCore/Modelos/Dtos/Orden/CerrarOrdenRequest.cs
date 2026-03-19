using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Orden
{
    public record CerrarOrdenRequest(
        [Required] int TecnicoId,
                   int? KmAlServicio,
        [Required, MaxLength(2000)] string TrabajosRealizados,
                   int? ProximoServicioKm,
                   DateOnly? ProximoServicioFecha,
        [Range(0, 365)] int GarantiaDias = 30
    );

    public record CerrarOrdenResponse(
        int HistorialId,
        int FacturaId,
        string NumeroFactura,
        int OrdenId,
        string GarantiaHasta
    );
}
