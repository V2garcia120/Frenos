namespace FrenosCore.Modelos.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public int? OrdenId { get; set; }
        public int ClienteId { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public string TipoOrigen { get; set; } = "OrdenReparacion";
        public string Estado { get; set; } = "Pendiente";
        public string MetodoPago { get; set; } = "Pendiente";
        public int EmitidaPor { get; set; }

        public Orden? Orden { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public Usuario EmitidaPorUsuario { get; set; } = null!;
        public CuentasPorCobrar? CuentasPorCorbrar { get; set; }
        public ICollection<FacturaItem> Items { get; set; } = [];
    }
}
