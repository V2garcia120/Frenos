namespace FrenosCore.Modelos.Entidades
{
    public class Orden
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int CotizacionId { get; set; }
        public string Estado { get; set; }
        public string Prioridad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateOnly FechaEntregaEstima { get; set; }
        public DateOnly FechaEntregaReal { get; set; }
        public string Notas { get; set; }

    }
}
