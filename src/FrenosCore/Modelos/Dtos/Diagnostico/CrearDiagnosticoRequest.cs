using System.ComponentModel.DataAnnotations;

namespace FrenosCore.Modelos.Dtos.Diagnostico
{
    public record CrearDiagnosticoRequest(
        [Required] int OrdenId,
        [Required] int TecnicoId,
                   int? KmIngreso,
        [Required, MaxLength(2000)] string DescripcionGeneral,
                   bool RequiereAtencionUrgente,
        [MaxLength(2000)] string? ObservacionesTecnico,
                   IEnumerable<CrearDiagnosticoItemRequest> Items
    );

    public record CrearDiagnosticoItemRequest(
        [Required, MaxLength(80)] string SistemaVehiculo,
        [Required, MaxLength(100)] string Componente,
        [Required] string Condicion,         
        [Required] string AccionRecomendada,  
        [MaxLength(2000)] string? Descripcion,
                                   int? ServicioSugeridoId,
                                   int? ProductoSugeridoId,
                                   bool EsUrgente,
                                   int CantidadProductoSugerida = 0
    );

}
