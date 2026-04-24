using FrenosIntegracion.Helpers;
using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/clientes")]
    [Authorize]
    public class ClientesController(ICoreService core) : ControllerBase
    {
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(ApiResponse<object>.Fail("VALIDATION_ERROR", "El parámetro q es requerido."));

            var token = ObtenerToken();
            var clientes = await core.BuscarClientesAsync(q, token);
            return Ok(ApiResponse<object>.Ok(clientes));
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }
    }
}
