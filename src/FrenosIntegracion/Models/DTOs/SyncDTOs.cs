using System.ComponentModel.DataAnnotations;

namespace FrenosIntegracion.Models.DTOs
{
    public record SyncRequest
    {
        [Required]
        public List<TransaccionPendienteDTO> Transacciones { get; init; } = new();
    }

    public record TransaccionPendienteDTO
    {
        [Required] public string IdLocal { get; init; } = string.Empty;
        [Required] public string Tipo { get; init; } = string.Empty;
        [Required] public string Payload { get; init; } = string.Empty;
        [Required] public DateTime Fecha { get; init; }
    }

    public record SyncResponse
    {
        public int Procesadas { get; init; }
        public int Fallidas { get; init; }
        public IEnumerable<SyncResultItem> Resultados { get; init; } = Enumerable.Empty<SyncResultItem>();
    }

    public record SyncResultItem
    {
        public string IdLocal { get; init; } = string.Empty;
        public bool Exito { get; init; }
        public int? FacturaId { get; init; }
        public string? MensajeError { get; init; }
    }
}