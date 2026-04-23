namespace TallerCaja.Models.DTOs
{
    public class AbonoRequest
    {
        public int TurnoId { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
    }
}
