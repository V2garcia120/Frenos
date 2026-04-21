using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Cache;
using FrenosIntegracion.Helpers; // Asegúrate de que ApiResponse esté aquí
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/catalogo")]
    [Authorize]
    public class CatalogoController(ICacheService cache) : ControllerBase
    {
        // 1. Catálogo de productos (Pág. 7-8)
        // GET /int/catalogo/productos?categoria=
        [HttpGet("productos")]
        public async Task<IActionResult> GetProductos([FromQuery] string? categoria)
        {
            var productos = await cache.ObtenerProductosAsync();

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                productos = productos.Where(p =>
                    p.Categoria?.Equals(categoria, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Formato de respuesta estándar (Pág. 14)
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(productos));
        }

        // 2. Catálogo de servicios (Pág. 8)
        // GET /int/catalogo/servicios
        [HttpGet("servicios")]
        public async Task<IActionResult> GetServicios()
        {
            var servicios = await cache.ObtenerServiciosAsync();

            // El documento exige devolver ID, Nombre, Precio, Duración y Categoría (Pág. 8)
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(servicios));
        }

        // 3. Búsqueda combinada del catálogo (Pág. 8)
        // GET /int/catalogo/buscar?q=
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? q)
        {
            // Validación según el documento (Pág. 8 y 14)
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "VALIDATION_ERROR",
                    "El término de búsqueda (q) es requerido."));
            }

            var termino = q.Trim().ToLower();

            // Busca simultáneamente en productos y servicios (Pág. 8)
            var todosProductos = await cache.ObtenerProductosAsync();
            var productosFiltrados = todosProductos
                .Where(p => p.Nombre.ToLower().Contains(termino))
                .ToList();

            var todosServicios = await cache.ObtenerServiciosAsync();
            var serviciosFiltrados = todosServicios
                .Where(s => s.Nombre.ToLower().Contains(termino))
                .ToList();

            // Devuelve objeto con ambas listas (Pág. 8)
            var resultado = new
            {
                productos = productosFiltrados,
                servicios = serviciosFiltrados
            };

            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }
    }
}