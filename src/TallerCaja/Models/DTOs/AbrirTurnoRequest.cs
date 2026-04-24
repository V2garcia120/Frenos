namespace TallerCaja.Models.DTOs
{
    public class AbrirTurnoRequest
    {
        public int TurnoLocalCaja { get; set; }
        public int CajeroId { get; set; }
        public decimal MontoInicial { get; set; }
    }
}
