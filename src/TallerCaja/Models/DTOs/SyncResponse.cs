namespace TallerCaja.Models.DTOs
{
    public class SyncResponse
    {
        public int Procesadas { get; set; }
        public int Fallidas { get; set; }
        public List<SyncResultadoDto> Resultados { get; set; } = new();
    }
}
