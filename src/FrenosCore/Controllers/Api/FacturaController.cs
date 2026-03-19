using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController(IFacturaService facturas) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Listar(
       [FromQuery] int pagina = 1,
       [FromQuery] int tam = 20,
       [FromQuery] string? estado = null)
        {
            var resultado = await facturas.ListarAsync(pagina, tam, estado);
            return Ok(ApiResponse<object>.Ok(resultado));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var factura = await facturas.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(factura));
        }

        [HttpPost("{id:int}/pago")]
        [Authorize(Policy = "Mantenimiento")]
        public async Task<IActionResult> RegistrarPago(
            int id, [FromBody] RegistrarPagoRequest request)
        {
            var factura = await facturas.RegistrarPagoAsync(id, request);
            return Ok(ApiResponse<object>.Ok(factura));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "SoloAdmin")]
        public async Task<IActionResult> Anular(int id)
        {
            await facturas.AnularAsync(id);
            return Ok(ApiResponse.Ok());
        }
    }
}
