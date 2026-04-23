namespace TallerCaja.Models.DTOs
{
    public class ItemCobroDto
    {
        public string Tipo { get; set; } = string.Empty; // Producto | Servicio
        public int ItemId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioSnapshot { get; set; }
        public string NombreSnapshot { get; set; } = string.Empty; // Solo uso local
    }
}
