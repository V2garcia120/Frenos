namespace TallerCaja.Models.DTOs
{
    public class CerrarTurnoResponse
    {
        public int TurnoId { get; set; }
        public decimal EfectivoEsperado { get; set; }
        public decimal EfectivoContado { get; set; }
        public decimal Diferencia { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
