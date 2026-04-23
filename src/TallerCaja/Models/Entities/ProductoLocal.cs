namespace TallerCaja.Models.Entities
{
    public class ProductoLocal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime UltimaSync { get; set; }
    }
}
