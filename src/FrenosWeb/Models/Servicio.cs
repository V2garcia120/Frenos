namespace FrenosWeb.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool RequiereVehiculo { get; set; } = true;
        public int Stock { get; set; }
        public string? Categoria { get; set; }
    }
}
