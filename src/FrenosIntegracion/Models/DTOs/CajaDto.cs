using System.ComponentModel.DataAnnotations;

namespace FrenosIntegracion.Models.DTOs
{
    public record AbrirTurnoRequest(
        [Required] int CajeroId,
        [Required] decimal MontoInicial
    );

    public record CobroRequest(
        [Required] int TurnoId,
        [Required] int ClienteId,
                   int? VehiculoId,
        [Required] IEnumerable<CobroItem> Items,
        [Required] string MetodoPago,
        [Required] decimal MontoPagado
    );

    public record CobroItem(
        [Required] string Tipo,
        [Required] int ItemId,
        [Required] int Cantidad,
        [Required] decimal PrecioSnapshot
    );

    public record CobroResponse(
        int? FacturaId,
        string? NumeroFactura,
        decimal Total,
        decimal Cambio,
        string Estado,
        string IdLocal
    );

    public record SyncResponse(
        int Procesadas,
        int Fallidas,
        IEnumerable<SyncResultItem> Resultados
    );

    public record HealthResponse(
        string Integracion,
        string Core,
        bool ModoCache,
        DateTime UltimaSync
    );
}
