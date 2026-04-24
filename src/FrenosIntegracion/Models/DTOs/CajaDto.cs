using System.ComponentModel.DataAnnotations;

namespace FrenosIntegracion.Models.DTOs
{
    // --- Autenticación ---
    public record LoginRequest(
        [Required][EmailAddress] string Email,
        [Required] string Password
    );

    // --- Gestión de Turnos ---
    public record AbrirTurnoRequest(
        int TurnoLocalCaja,
        [Required] int CajeroId,
        [Required] decimal MontoInicial
    );

    public record CerrarTurnoRequest(
        [Required] int TurnoId,
        [Required] decimal EfectivoContado,
        string? Observaciones
    );

    // --- Movimientos de Efectivo ---
    public record MovimientoEfectivoRequest(
        [Required] int TurnoId,
        [Required] decimal Monto,
        [Required] string Motivo
    );

    // --- Procesamiento de Cobros ---
    public record CobroRequest(
        [Required] int TurnoId,
        [Required] int ClienteId,
        int? VehiculoId,
        [Required] IEnumerable<CobroItem> Items,
        [Required] string MetodoPago,
        [Required] decimal MontoPagado
    );

    public record CobroItem(
        [Required] string Tipo, // "Producto" o "Servicio"
        [Required] int ItemId,
        [Required] int Cantidad,
        [Required] decimal PrecioSnapshot
    );

    public record CobroResponse(
        int? FacturaId,
        string? NumeroFactura,
        decimal Total,
        decimal Cambio,
        string Estado, // "Completado" o "PendienteSync"
        string IdLocal
    );

    // --- Pagos de Factura y CxC ---
    public record PagoFacturaRequest(
        [Required] int FacturaId,
        [Required] int TurnoId,
        [Required] string MetodoPago,
        [Required] decimal Monto
    );

    public record AbonoCxCRequest(
        [Required] int FacturaId,
        [Required] int TurnoId,
        [Required] decimal Monto,
        [Required] string MetodoPago
    );

    // --- Sincronización Offline (Pág. 13 del Manual) ---
    public record SyncRequest(
        [Required] List<TransaccionPendienteDTO> transacciones
    );

    public record TransaccionPendienteDTO(
        [Required] string idLocal,
        [Required] string tipo,
        [Required] string payload, // JSON de la operación
        [Required] DateTime fecha
    );

    public record SyncResponse(
        int Procesadas,
        int Fallidas,
        IEnumerable<SyncResultItem> Resultados
    );

    public record SyncResultItem(
        string IdLocal,
        bool Exito,
        int? FacturaId,
        string? MensajeError
    );

    // --- Monitoreo ---
    public record HealthResponse(
        string Integracion,
        string Core,
        bool ModoCache,
        DateTime UltimaSync
    );
}