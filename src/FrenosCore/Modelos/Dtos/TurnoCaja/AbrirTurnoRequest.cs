namespace FrenosCore.Modelos.Dtos.TurnoCaja
{
    public record AbrirTurnoRequest
    (
        int CajeroId,
        int TurnoLocalCaja,
        decimal MontoInicial
    );
}
