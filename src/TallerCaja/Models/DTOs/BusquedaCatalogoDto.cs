namespace TallerCaja.Models.DTOs
{
    public class BusquedaCatalogoDto
    {
        public List<ProductoDto> Productos { get; set; } = new();
        public List<ServicioDto> Servicios { get; set; } = new();
    }
}
