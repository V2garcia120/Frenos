using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/ordenes")]
    [Authorize]
    public class OrdenesController(ICoreService core) : ControllerBase
    {
        // GET: int/ordenes/historial
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial()
        {
            try
            {
                // Llamamos al método que ya definiste en ICoreService
                var historial = await core.ObtenerHistorialOrdenesAsync();

                if (historial == null)
                {
                    return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(new List<object>()));
                }

                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(historial));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE", "No se pudo obtener el historial. " + ex.Message));
            }
        }
    }
}