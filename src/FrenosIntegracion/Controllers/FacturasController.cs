using FrenosIntegracion.Helpers;
using FrenosIntegracion.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/facturas")]
    [Authorize]
    public class FacturasController(ICoreService core) : ControllerBase
    {
        [HttpGet("mis-facturas")]
        public async Task<IActionResult> MisFacturas()
        {
            try
            {
                var token = ObtenerToken();
                var lista = await core.ObtenerMisFacturasAsync(token);
                return Ok(ApiResponse<object>.Ok(lista ?? []));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ApiResponse<object>.Fail("CORE_UNAVAILABLE", ex.Message));
            }
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }
    }
}
