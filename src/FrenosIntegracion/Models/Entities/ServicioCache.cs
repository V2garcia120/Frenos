namespace FrenosIntegracion.Models.Entities
{
    public class ServicioCache
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int? DuracionMin { get; set; }
        public string? Categoria { get; set; }
        public bool Activo { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
