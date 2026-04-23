namespace TallerCaja.Models.DTOs
{
    public class AbrirTurnoResponse
    {
        public int TurnoId { get; set; }
        public string Estado { get; set; } = "Abierto";
        public DateTime FechaApertura { get; set; }
    }
}
