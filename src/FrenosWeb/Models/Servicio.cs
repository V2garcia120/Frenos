namespace FrenosWeb.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public bool RequiereVehiculo { get; set; } = true;
    }
}
