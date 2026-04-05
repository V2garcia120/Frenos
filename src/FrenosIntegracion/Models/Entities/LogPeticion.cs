namespace FrenosIntegracion.Models.Entities
{
    public class LogPeticion
    {
        public long Id { get; set; }
        public string Canal { get; set; } = string.Empty; // Web | Caja
        public string Metodo { get; set; } = string.Empty; // GET, POST...
        public string Endpoint { get; set; } = string.Empty; // /api/catalogo...
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public int DuracionMs { get; set; }
        public bool CoreAlcanzado { get; set; }
        public string? IP { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}
