namespace FrenosWeb.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string? Duracion { get; set; }
        public bool RequiereVehiculo { get; set; } = false; 
        public int Stock { get; set; }
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
        public string TipoItem { get; set; } = ""; 
    }
}
