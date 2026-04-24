namespace TallerCaja.Models.DTOs
{
    public class PagoFacturaResponse
    {
        public int FacturaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Cambio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? CxcId { get; set; }
    }
}
