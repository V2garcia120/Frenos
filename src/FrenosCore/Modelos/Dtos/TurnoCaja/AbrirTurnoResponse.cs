namespace FrenosCore.Modelos.Dtos.TurnoCaja
{
    public record AbrirTurnoResponse(

        int TurnoId,
        string Estado,
        DateTime Fechaapertura
    );
}
