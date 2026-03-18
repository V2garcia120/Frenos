using FrenosCore.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrenosCore.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FrenosCore.Modelos.Dtos.Cliente;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClientesController(IClienteService clienteService) => _clienteService = clienteService;

        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int pagina = 1,
            [FromQuery] int tam = 20,
            [FromQuery] string? busqueda = null)
        {
            var clientes = await _clienteService.ListarAsync(pagina, tam, busqueda);
            return Ok(ApiResponse<object>.Ok(clientes));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(cliente));
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return BadRequest(ApiResponse<object>.Fail(
                    "VALIDATION_ERROR", "El parámetro q es requerido."));

            var clientes = await _clienteService.BuscarAsync(termino);
            return Ok(ApiResponse<object>.Ok(clientes));
        }

        [HttpGet("anonimo")]
        public async Task<IActionResult> ObtenerClienteAnonimo()
        {
            var cliente = await _clienteService.ObtenerAnonimoAsync();
            return Ok(ApiResponse<object>.Ok(cliente));
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClienteRequest request)
        {
            var cliente = await _clienteService.CrearAsync(request);
            return Ok(ApiResponse<object>.Ok(cliente));
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarClienteRequest request)
        {
            var cliente = await _clienteService.ActualizarAsync(id, request);
            return Ok(ApiResponse<object>.Ok(cliente));
        }
         [HttpDelete("{id:int}")]
         public async Task<IActionResult> Eliminar(int id)
         {
             var resultado = await _clienteService.EliminarAsync(id);
             if (!resultado)
                 return NotFound(ApiResponse<object>.Fail("NOT_FOUND", $"Cliente con ID {id} no encontrado."));
             return Ok(ApiResponse<object>.Ok($"Cliente con ID {id} eliminado exitosamente."));
         }
    }
}
