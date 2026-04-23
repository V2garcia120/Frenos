namespace TallerCaja.Models.DTOs
{
    public class PagoFacturaRequest
    {
        public int TurnoId { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
    }
}
