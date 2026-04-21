namespace FrenosWeb.Models
{
    public class CarritoItem
    {
        public Producto Producto { get; set; } = new();
        public Servicio Servicio { get; set; } = new();
        public int Cantidad { get; set; } = 1;
        public decimal TotalLinea => Producto.Precio * Cantidad;
    }
}
