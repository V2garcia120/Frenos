namespace FrenosCore.Modelos.Entidades
{
    public class Diagnostico
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int TecnicoId { get; set; }
        public int? KmIngreso { get; set; }
        public string DescripcionGeneral { get; set; }
        public string Estado { get; set; } = "Borrador";
        public bool RequiereAtencionUrgente { get; set; }
        public bool AprobadoPorCliente { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? ObservacionesTecnico { get; set; }
        public DateTime FechaDiagnostico { get; set; } = DateTime.Now;

        public Orden Orden { get; set; } = null!;
        public Usuario Tecnico { get; set; } = null!;
        public ICollection<DiagnosticoItem> Items { get; set; } = [];
    }
}
