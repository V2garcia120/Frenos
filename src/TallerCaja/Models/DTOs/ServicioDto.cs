namespace TallerCaja.Models.DTOs
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMin { get; set; } = 0;
        public string Categoria { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
