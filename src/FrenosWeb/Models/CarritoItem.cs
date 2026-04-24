namespace FrenosWeb.Models
{
    public class CarritoItem
    {
        public Servicio Servicio { get; set; } = new();
        public int Cantidad { get; set; } = 1;
        public decimal PrecioSnapshot => Servicio.Precio;
        public decimal TotalLinea => PrecioSnapshot * Cantidad;
    }
}
