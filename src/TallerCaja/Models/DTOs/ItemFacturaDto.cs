namespace TallerCaja.Models.DTOs
{
    public class ItemFacturaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
