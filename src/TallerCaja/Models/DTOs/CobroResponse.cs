namespace TallerCaja.Models.DTOs
{
    public class CobroResponse
    {
        public int? FacturaId { get; set; }
        public string? NumeroFactura { get; set; }
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = string.Empty; // Completado | PendienteSync
        public string IdLocal { get; set; } = string.Empty;
    }
}
