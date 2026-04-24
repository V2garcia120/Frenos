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
                var token = ObtenerToken();
                var historial = await core.ObtenerHistorialOrdenesAsync(token);

                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(historial ?? []));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE", "No se pudo obtener el historial. " + ex.Message));
            }
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }
    }
}