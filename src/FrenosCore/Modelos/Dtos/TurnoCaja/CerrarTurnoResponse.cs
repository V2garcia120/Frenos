namespace FrenosCore.Modelos.Dtos.TurnoCaja
{

    public record CerrarTurnoResponse
    (
        int TurnoId,
        string Estado,
        DateTime FechaApertura,
        DateTime FechaCierre,
        decimal MontoInicial,
        decimal EfectivoContado,
        string Observaciones
    );
}
