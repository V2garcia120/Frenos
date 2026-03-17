namespace FrenosCore.Modelos.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimoLogin { get; set; }

        public Rol Rol { get; set; } = null!;
        public ICollection<AbonoCxC> AbonosRegistrados { get; set; } = [];
        public ICollection<Diagnostico> DiagnosticosAsignados { get; set; } = [];
        public ICollection<HistorialReparaciones> HistorialesReparacionTecnico { get; set; } = [];
        public ICollection<Factura> FacturasEmitidas { get; set; } = [];

    }
}
