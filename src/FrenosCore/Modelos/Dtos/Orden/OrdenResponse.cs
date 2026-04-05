using FrenosCore.Modelos.Dtos.Diagnostico;

namespace FrenosCore.Modelos.Dtos.Orden
{
    public record OrdenResponse(
        int Id,
        int ClienteId,
        string ClienteNombre,
        int VehiculoId,
        string VehiculoInfo,
        int? TecnicoId,
        string? TecnicoNombre,
        int? CotizacionId,
        string Estado,
        string Prioridad,
        DateTime FechaIngreso,
        DateTime? FechaEntregaEstimada,
        DateTime? FechaEntregaReal,
        string? Notas,
        bool TieneDiagnostico,
        bool TieneFactura
    );

    public record OrdenDetalleResponse(
        int Id,
        int ClienteId,
        string ClienteNombre,
        int VehiculoId,
        string VehiculoInfo,
        int? TecnicoId,
        string? TecnicoNombre,
        int? CotizacionId,
        string Estado,
        string Prioridad,
        DateTime FechaIngreso,
        DateTime? FechaEntregaEstimada,
        DateTime? FechaEntregaReal,
        string? Notas,
        DiagnosticoResponse? Diagnostico,
        int? FacturaId
    );
}
