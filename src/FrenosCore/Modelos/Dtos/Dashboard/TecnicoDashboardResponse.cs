namespace FrenosCore.Modelos.Dtos.Dashboard
{
    public record TecnicoDashboardResponse(
        int MisOrdenesHoy,
        int OrdenesUrgentesHoy,
        int EnProgresoAhora,
        int CompletadasHoy,
        IReadOnlyList<TecnicoColaTrabajoItem> ColaTrabajo,
        IReadOnlyList<TecnicoCompletadaItem> Completadas
    );

    public record TecnicoColaTrabajoItem(
        int OrdenId,
        string Estado,
        string Cliente,
        string Vehiculo,
        string Placa,
        string Detalle,
        DateTime FechaIngreso,
        DateTime? FechaEntregaEstimada,
        bool EsUrgente
    );

    public record TecnicoCompletadaItem(
        int OrdenId,
        string Cliente,
        string Vehiculo,
        string Placa,
        string Detalle,
        DateTime FechaCompletada
    );
}
