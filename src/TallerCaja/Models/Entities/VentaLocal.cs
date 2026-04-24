namespace TallerCaja.Models.Entities
{
    public class VentaLocal
    {
        public int Id { get; set; }
        public string IdLocal { get; set; } = Guid.NewGuid().ToString();
        public int? FacturaIdCore { get; set; }
        public string? NumeroFactura { get; set; }
        public int TurnoId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal ITBIS { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = "Completado"; // Completado | PendienteSync
        public DateTime FechaVenta { get; set; }
    }
}
