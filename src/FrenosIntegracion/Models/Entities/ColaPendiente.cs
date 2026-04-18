using System.ComponentModel.DataAnnotations;

namespace FrenosIntegracion.Models.Entities
{
    public class ColaPendiente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string IdLocal { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Canal { get; set; } = null!; // "Caja", "Web", etc.

        [Required]
        [StringLength(50)]
        public string TipoOperacion { get; set; } = null!; // "cobro", "orden"

        // ESTA ES LA QUE TE FALTA:
        [Required]
        public string PayloadJson { get; set; } = null!; 

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        public int Intentos { get; set; } = 0;
        public int MaxIntentos { get; set; } = 5;

        public string? ErrorDetalle { get; set; }
        public string? RespuestaCore { get; set; }

        public DateTime? ProximoIntento { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaProcesado { get; set; }
    }
}