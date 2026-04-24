namespace TallerCaja.Models.DTOs
{
    public class SyncResultadoDto
    {
        public string IdLocal { get; set; } = string.Empty;
        public bool Exitosa { get; set; }
        public string? Error { get; set; }
    }
}
