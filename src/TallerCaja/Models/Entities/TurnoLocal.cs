namespace TallerCaja.Models.Entities
{
    public class TurnoLocal
    {
        public int Id { get; set; }
        public int CajeroId { get; set; }
        public string CajeroNombre { get; set; } = string.Empty;
        public decimal MontoInicial { get; set; }
        public decimal? EfectivoContado { get; set; }
        public string Estado { get; set; } = "Abierto"; // Abierto | Cerrado
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? Observaciones { get; set; }
        public int? TurnoIdCore { get; set; }
    }
}
