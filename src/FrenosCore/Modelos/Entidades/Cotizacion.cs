namespace FrenosCore.Modelos.Entidades
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Itbis { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string Notas { get; set; } = string.Empty;
        public DateTime ValidaHasta { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Vehiculo Vehiculo { get; set; } = null!;
        public ICollection<CotizacionItem> Items { get; set; } = [];
        public Orden Orden { get; set; } = null!;
    }
}
