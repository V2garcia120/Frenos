namespace FrenosCore.Modelos.Entidades
{
    public class HistorialReparaciones
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public int OrdenId { get; set; }
        public int TecnicoId { get; set; }
        public int KmAlServicio { get; set; }
        public string TrabajosRealizados { get; set; }
        public int ProximoServicioKm { get; set; }
        public DateOnly ProximoServicioFecha { get; set; }
        public int GarantiaDias { get; set; }
        public DateOnly GarantiaHasta { get; set; }
        public DateTime Fecha { get; set; }

        public Vehiculo Vehiculo { get; set; } = null!;
        public Orden Orden { get; set; } = null!;
        public Usuario Tecnico { get; set; } = null!;

    }
}
