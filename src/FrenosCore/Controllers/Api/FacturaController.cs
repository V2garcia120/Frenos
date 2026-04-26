using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController(IFacturaService facturas, AppDbContext db) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "Cajero")]
        public async Task<IActionResult> CobrarDirecto([FromBody] CobroDirectoRequest request)
        {
            var resultado = await facturas.CobrarDirectoAsync(request);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

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

        [HttpGet("mis-ordenes")]
        [Authorize]
        public async Task<IActionResult> MisOrdenes()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub");

            Console.WriteLine($"[MisOrdenes] sub={sub}");

            if (!int.TryParse(sub, out var clienteId) || clienteId == 0)
                return Unauthorized(ApiResponse<object>.Fail("AUTH_ERROR", "Cliente no identificado."));

            // Órdenes de servicio
            var ordenes = await db.Orden
                .AsNoTracking()
                .Include(o => o.Vehiculo)
                .Where(o => o.ClienteId == clienteId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            // Facturas de venta directa (productos)
            var facturas = await db.Factura
                .AsNoTracking()
                .Include(f => f.Items)
                .Where(f => f.ClienteId == clienteId && f.TipoOrigen == "VentaDirecta")
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            var resultado = ordenes.Select(o => new
            {
                NumeroOrden = $"ORD-{o.Id:D4}",
                Fecha = o.FechaCreacion,
                TipoServicio = o.Notas?.Contains("Items:") == true
        ? o.Notas.Split("Items:").Last().Split('|').First().Trim()
        : o.Notas?.Split('|').FirstOrDefault()?.Trim() ?? "Servicio de taller",
                Vehiculo = o.Vehiculo != null ? $"{o.Vehiculo.Marca} {o.Vehiculo.Modelo} {o.Vehiculo.Anno}" : "N/A",
                Placa = o.Vehiculo?.Placa ?? "N/A",
                Total = ParsearTotal(o.Notas),
                EstadoServicio = o.Estado == "Recibido" ? "Recibido" : o.Estado,  
                EsServicio = o.VehiculoId != null  
            }).Concat(facturas.Select(f => new
            {
                NumeroOrden = f.Numero,
                Fecha = f.Fecha,
                TipoServicio = "Venta: " + string.Join(", ", f.Items.Take(2).Select(i => i.Descripcion)),
                Vehiculo = "N/A",
                Placa = "N/A",
                Total = f.Total,
                EstadoServicio = f.Estado,  // ← "Pagada" para Tarjeta/Transferencia
                EsServicio = false
            })).OrderByDescending(x => x.Fecha);

            return Ok(ApiResponse<object>.Ok(resultado));
        }

        private static decimal ParsearTotal(string? notas)
        {
            if (string.IsNullOrEmpty(notas)) return 0;
            var parte = notas.Split("Total: RD$").LastOrDefault();
            return decimal.TryParse(parte?.Replace(",", "").Trim(), out var total) ? total : 0;
        }

        [HttpGet("mis-facturas")]
        [Authorize]
        public async Task<IActionResult> MisFacturas()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(sub, out var clienteId) || clienteId == 0)
                return Unauthorized(ApiResponse<object>.Fail("AUTH_ERROR", "Cliente no identificado."));

            var lista = await facturas.ObtenerMisFacturasAsync(clienteId);
            return Ok(ApiResponse<object>.Ok(lista));
        }

        private static string LimpiarNotas(string? notas)
        {
            if (string.IsNullOrEmpty(notas)) return "Servicio de taller";
            var partes = notas.Split('|');
            var metodoPago = partes[0].Replace("Orden web - Pago:", "").Trim();
            return $"Servicio de taller ({metodoPago})";
        }
    }
}
