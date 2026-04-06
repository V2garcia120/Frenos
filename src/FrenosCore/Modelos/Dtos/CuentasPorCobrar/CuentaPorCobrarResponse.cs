namespace FrenosCore.Modelos.Dtos.CuentasPorCobrar
{
    public record CuentaPorCobrarResponse(
        int Id,
        int ClienteId,
        string ClienteNombre,
        int FacturaId,
        string NumeroFactura,
        decimal Monto,
        decimal Saldo,
        DateTime Vencimiento,
        string Estado,
        DateTime CreadoEn
    );
}
