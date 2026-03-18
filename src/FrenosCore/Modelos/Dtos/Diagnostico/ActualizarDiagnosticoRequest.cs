namespace FrenosCore.Modelos.Dtos.Diagnostico
{
    public record ActualizarDiagnosticoRequest(
        int? TecnicoId,
        int? KmIngerso,
        string? DescripcionGeneral,
        string? Estado,
        bool? RequiereAtencionUrgente,
        bool? AprobadoPorCliente,
        DateTime? FechaAprobacion,
        string? ObservacionesTecnico
    );
}
