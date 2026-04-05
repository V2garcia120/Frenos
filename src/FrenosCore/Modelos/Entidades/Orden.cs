namespace FrenosCore.Modelos.Entidades
{
    public class Orden
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int? TecnicoId { get; set; }
        public int? CotizacionId { get; set; }
        public string Estado { get; set; } = "Recibido";
        public string Prioridad { get; set; } = "Normal";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEntregaEstima { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public string Notas { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Vehiculo Vehiculo { get; set; } = null!;
        public Usuario? TecnicoAsignado { get; set; }
        public Cotizacion Cotizacion { get; set; } = null!;
        public Diagnostico Diagnostico { get; set; } = null!;
        public Factura Factura { get; set; } = null!;

    }
}
