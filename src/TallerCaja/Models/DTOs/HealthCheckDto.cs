namespace TallerCaja.Models.DTOs
{
    public class HealthCheckDto
    {
        public string Integracion { get; set; } = "online";
        public string Core { get; set; } = "offline";
        public bool ModoCache { get; set; }
        public DateTime? UltimaSync { get; set; }
    }
}
