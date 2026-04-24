using System.Text.Json.Serialization;

namespace FrenosIntegracion.Models.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Categoria { get; set; }
        public bool Activo { get; set; }
    }

    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int? DuracionMin { get; set; }
        public string? Categoria { get; set; }
        public bool Activo { get; set; }
    }

    public record BusquedaCatalogoResponse(
        IEnumerable<ProductoDto> Productos,
        IEnumerable<ServicioDto> Servicios
    );

    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool EsAnonimo { get; set; }
    }
}