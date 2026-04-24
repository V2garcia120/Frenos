namespace FrenosCore.Modelos.Dtos.TurnoCaja
{
    public record AbrirTurnoRequest
    (
        int CajeroId,
        decimal MontoInicial
    );
}
