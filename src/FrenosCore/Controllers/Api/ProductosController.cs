using FrenosCore.Servicios;
using FrenosCore.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? categoria = null)
        {
            var productos = await _productoService.ListarTodosAsync(categoria);
            return Ok(ApiResponse<object>.Ok(productos));
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest(ApiResponse<object>.Fail(
                    "VALIDATION_ERROR", "El parámetro termino es requerido."));

            var productos = await _productoService.BuscarAsync(termino);
            return Ok(ApiResponse<object>.Ok(productos));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(producto));
        }
    }
}
