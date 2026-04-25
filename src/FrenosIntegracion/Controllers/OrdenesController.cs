using FrenosIntegracion.Models.DTOs;
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
                return Ok(ApiResponse<object>.Ok(historial ?? []));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE", ex.Message));
            }
        }

        // POST: int/ordenes/web 
        [HttpPost("web")]
        public async Task<IActionResult> CrearOrdenWeb([FromBody] CrearOrdenWebRequest request)
        {
            try
            {
                var token = ObtenerToken();
                var orden = await core.CrearOrdenAsync(request, token);
                return Ok(ApiResponse<object>.Ok(orden));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail(
                    "CORE_UNAVAILABLE", ex.Message));
            }
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }
    }
}