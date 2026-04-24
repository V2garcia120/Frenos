namespace TallerCaja.Models.DTOs
{
    public class CerrarTurnoRequest
    {
        public int TurnoId { get; set; }
        public decimal EfectivoContado { get; set; }
        public string? Observaciones { get; set; }
    }
}
