using System.Security.Claims;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/vehiculos")]
    [Authorize]
    public class VehiculosController : ControllerBase
    {
        private readonly ICoreService _core;

        public VehiculosController(ICoreService core)
        {
            _core = core;
        }

        [HttpGet("mis-vehiculos")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                // El Core pone el clienteId en el claim "sub"
                // .NET lo mapea automáticamente a NameIdentifier
                var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(sub, out var clienteId) || clienteId <= 0)
                    return Unauthorized(ApiResponse<object>.Fail(
                        "AUTH_ERROR", "No se pudo identificar al cliente."));

                var lista = await _core.ObtenerVehiculosClienteAsync(clienteId);
                return Ok(ApiResponse<object>.Ok(lista));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] object vehiculo)
        {
            try
            {
                var nuevo = await _core.RegistrarVehiculoAsync(vehiculo);
                return Ok(ApiResponse<object>.Ok(nuevo));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] object vehiculo)
        {
            try
            {
                var actualizado = await _core.ActualizarVehiculoAsync(id, vehiculo);
                return Ok(ApiResponse<object>.Ok(actualizado));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _core.EliminarVehiculoAsync(id);
                return Ok(ApiResponse<object>.Ok($"Vehículo {id} eliminado."));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }
    }
}