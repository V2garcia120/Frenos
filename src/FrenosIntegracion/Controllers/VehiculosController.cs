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

        // GET: int/vehiculos/mis-vehiculos?clienteId=5
        [HttpGet("mis-vehiculos")]
        public async Task<IActionResult> Listar([FromQuery] int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                    return BadRequest(ApiResponse<object>.Fail("VALIDATION_ERROR", "clienteId requerido."));

                var lista = await _core.ObtenerVehiculosClienteAsync(clienteId);
                return Ok(ApiResponse<object>.Ok(lista));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }

        // POST: int/vehiculos
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

        // PUT: int/vehiculos/{id}
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

        // DELETE: int/vehiculos/{id}
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