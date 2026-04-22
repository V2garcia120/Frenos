using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/vehiculos")]
    [Authorize]
    public class VehiculosController(ICoreService core) : ControllerBase
    {
        // GET: int/vehiculos/mis-vehiculos
        [HttpGet("mis-vehiculos")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await core.ObtenerVehiculosClienteAsync();
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(lista));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }

        // POST: int/vehiculos
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] object vehiculo)
        {
            try
            {
                var nuevo = await core.RegistrarVehiculoAsync(vehiculo);
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(nuevo));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail("CORE_ERROR", ex.Message));
            }
        }
    }
}