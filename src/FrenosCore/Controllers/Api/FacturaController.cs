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
        [Authorize]
        public async Task<IActionResult> Listar(
       [FromQuery] int pagina = 1,
       [FromQuery] int tam = 20,
       [FromQuery] string? estado = null,
       [FromQuery] string? numero = null,
       [FromQuery] DateTime? fecha = null,
       [FromQuery] string? tipoOrigen = null)
        {
            var resultado = await facturas.ListarAsync(pagina, tam, estado, numero, fecha, tipoOrigen);
            return Ok(ApiResponse<object>.Ok(resultado));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var factura = await facturas.ObtenerPorIdAsync(id);
            return Ok(ApiResponse<object>.Ok(factura));
        }

        [HttpPost("{id:int}/pago")]
        [Authorize(Policy = "Cajero")]
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

        [HttpGet("buscar")]
        [Authorize]
        public async Task<IActionResult> Buscar([FromQuery] string? placa = null, [FromQuery] string? numeroFactura = null)
        {
            var factura = await facturas.ObtenerFacturaPendientesAsync(placa, numeroFactura);
            return Ok(ApiResponse<object>.Ok(factura));
        }
    }
}
