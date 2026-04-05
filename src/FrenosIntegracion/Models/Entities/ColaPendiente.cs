namespace FrenosIntegracion.Models.Entities
{
    public class ColaPendiente
    {
        public long Id { get; set; }
        public string IdLocal { get; set; } = string.Empty; // UUID de la Web o Caja
        public string Canal { get; set; } = string.Empty; // Web | Caja
        public string TipoOperacion { get; set; } = string.Empty; // cobro | orden
        public string Payload { get; set; } = string.Empty; // El JSON con los datos
        public int Intentos { get; set; } = 0;
        public int MaxIntentos { get; set; } = 5;
        public string Estado { get; set; } = "Pendiente";
        public string? RespuestaCore { get; set; }
        public string? ErrorDetalle { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaProcesado { get; set; }
        public DateTime? ProximoIntento { get; set; }
    }
}
