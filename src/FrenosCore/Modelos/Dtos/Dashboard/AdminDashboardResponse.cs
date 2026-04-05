namespace FrenosCore.Modelos.Dtos.Dashboard
{
    public record AdminDashboardResponse(
        decimal FacturadoHoy,
        decimal FacturadoAyer,
        decimal FacturadoMes,
        decimal FacturadoMesAnterior,
        decimal CxcPendiente,
        int CuentasVencidas,
        int OrdenesActivas,
        int OrdenesUrgentes,
        IReadOnlyList<DashboardAlertaItem> Alertas,
        IReadOnlyList<DashboardServicioSolicitadoItem> ServiciosMasSolicitados,
        IReadOnlyList<DashboardEstadoOrdenItem> EstadoOrdenes,
        IReadOnlyList<DashboardCuentaPorCobrarItem> CuentasPorCobrarProximas
    );

    public record DashboardAlertaItem(string Tipo, string Titulo, string Detalle);

    public record DashboardServicioSolicitadoItem(string Nombre, int Cantidad);

    public record DashboardEstadoOrdenItem(string Estado, int Cantidad);

    public record DashboardCuentaPorCobrarItem(string Cliente, decimal Saldo, DateTime Vencimiento);
}
