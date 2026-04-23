namespace TallerCaja.Models.DTOs
{
    public class SyncItemDto
    {
        public string IdLocal { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
