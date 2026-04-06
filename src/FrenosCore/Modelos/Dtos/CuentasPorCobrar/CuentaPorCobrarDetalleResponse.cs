namespace FrenosCore.Modelos.Dtos.CuentasPorCobrar
{
    public record CuentaPorCobrarDetalleResponse(
        int Id,
        int ClienteId,
        string ClienteNombre,
        int FacturaId,
        string NumeroFactura,
        decimal Monto,
        decimal Saldo,
        DateTime Vencimiento,
        string Estado,
        DateTime CreadoEn,
        IReadOnlyList<AbonoCxCResponse> Abonos
    );

    public record AbonoCxCResponse(
        int Id,
        decimal Monto,
        DateTime Fecha,
        string MetodoPago,
        int RegistradoPor,
        string RegistradoPorNombre
    );
}
