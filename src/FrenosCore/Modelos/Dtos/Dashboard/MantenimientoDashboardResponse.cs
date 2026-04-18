namespace FrenosCore.Modelos.Dtos.Dashboard
{
    public record MantenimientoDashboardResponse(
        int OrdenesHoy,
        int OrdenesUrgentesHoy,
        int OrdenesListasHoy,
        int PendientesAprobar,
        int ListasSinEntregar,
        IReadOnlyList<MantenimientoAccionItem> RequierenAccion,
        IReadOnlyList<MantenimientoOrdenItem> OrdenesHoyDetalle
    );

    public record MantenimientoAccionItem(
        int OrdenId,
        string Estado,
        string Cliente,
        string VehiculoPlaca,
        string Descripcion,
        DateTime FechaReferencia
    );

    public record MantenimientoOrdenItem(
        int OrdenId,
        string Estado,
        string Cliente,
        string VehiculoPlaca,
        string EstadoDetalle,
        bool EsUrgente
    );
}
