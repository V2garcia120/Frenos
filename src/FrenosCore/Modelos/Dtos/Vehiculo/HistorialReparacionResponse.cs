namespace FrenosCore.Modelos.Dtos.Vehiculo
{
    public record HistorialReparacionResponse(
        int Id,
        int VehiculoId,
        int OrdenId,
        int TecnicoId,
        string TecnicoNombre,
        int? KmAlServicio,
        string TrabajosRealizados,
        int? ProximoServicioKm,
        DateOnly? ProximoServicioFecha,
        int GarantiaDias,
        DateOnly GarantiaHasta,
        DateTime Fecha);
}
