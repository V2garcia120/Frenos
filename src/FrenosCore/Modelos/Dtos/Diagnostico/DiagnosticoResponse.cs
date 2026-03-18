namespace FrenosCore.Modelos.Dtos.Diagnostico
{
    public record DiagnosticoResponse(
        int Id,
        int OrdenId,
        string VehiculoInfo,     
        string ClienteNombre,
        int TecnicoId,
        string TecnicoNombre,
        int? KmIngreso,
        string DescripcionGeneral,
        string Estado,
        bool RequiereAtencionUrgente,
        bool AprobadoPorCliente,
        DateTime? FechaAprobacion,
        string? ObservacionesTecnico,
        DateTime FechaDiagnostico,
        IEnumerable<DiagnosticoItemResponse> Items
    );

    public record DiagnosticoItemResponse(
        int Id,
        string SistemaVehiculo,
        string Componente,
        string Condicion,
        string AccionRecomendada,
        string? Descripcion,
        int? ServicioSugeridoId,
        string? ServicioSugeridoNombre,
        int? ProductoSugeridoId,
        string? ProductoSugeridoNombre,
        bool EsUrgente,
        int CantidadProductoSugerido = 0
    );


}
