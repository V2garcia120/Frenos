namespace TallerCaja.Models.DTOs
{
    public class CobroRequest
    {
        public int TurnoId { get; set; }
        public int ClienteId { get; set; }
        public int? VehiculoId { get; set; }
        public List<ItemCobroDto> Items { get; set; } = new();
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
    }
}
