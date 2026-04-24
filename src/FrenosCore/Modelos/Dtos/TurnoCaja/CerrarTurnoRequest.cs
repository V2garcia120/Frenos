namespace FrenosCore.Modelos.Dtos.TurnoCaja
{
    public record CerrarTurnoRequest
    (
        int TurnoId,
        decimal MontoFinal,
        string Observaciones
    );
}
