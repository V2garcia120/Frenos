using System.ComponentModel.DataAnnotations;

namespace FrenosIntegracion.Models.DTOs
{
    public record CrearOrdenWebRequest(
        [Required] int ClienteId,
                   int? VehiculoId,
                   string? VehiculoInfo,
        [Required] IEnumerable<OrdenWebItem> Items,
        [MaxLength(500)] string? Notas
    );

    public record OrdenWebItem(
        [Required] string Tipo,      // Producto | Servicio
        [Required] int ItemId,
        [Required] int Cantidad,
        [Required] decimal PrecioSnapshot
    );

    public record OrdenWebResponse(
        int OrdenWebId,
        int? OrdenCoreId,
        string Estado,
        decimal Total,
        string Mensaje
    );

    public record EstadoOrdenResponse(
        int OrdenId,
        string Estado,
        string VehiculoInfo,
        DateTime FechaIngreso,
        DateTime? FechaEntregaEstimada
    );

}
