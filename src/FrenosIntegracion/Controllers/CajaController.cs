using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Services.Sync;
using FrenosIntegracion.Helpers; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/caja")]
    [Authorize]
    // Usamos Primary Constructor: 'core' y 'cola' son accesibles en toda la clase
    public class CajaController(ICoreService core, IColaSyncService cola) : ControllerBase
    {
        // 1. Abrir turno de caja
        [HttpPost("turno/abrir")]
        public async Task<IActionResult> AbrirTurno([FromBody] AbrirTurnoRequest request)
        {
            var resultado = await core.AbrirTurnoAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 2. Cerrar turno de caja
        [HttpPost("turno/cerrar")]
        public async Task<IActionResult> CerrarTurno([FromBody] CerrarTurnoRequest request)
        {
            var resultado = await core.CerrarTurnoAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 3. Entrada y salida de efectivo
        [HttpPost("efectivo/entrada")]
        public async Task<IActionResult> EntradaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        [HttpPost("efectivo/salida")]
        public async Task<IActionResult> SalidaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 4. Procesar cobro de venta directa
        [HttpPost("cobro")]
        public async Task<IActionResult> Cobro([FromBody] CobroRequest request)
        {
            try
            {
                var resultado = await core.ProcesarCobroAsync(request, ObtenerToken());
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch
            {
                var idLocal = await cola.EncolarOperacionAsync("Caja", "cobro", request);

                var total = request.Items.Sum(i => i.Cantidad * i.PrecioSnapshot);
                var response = new
                {
                    facturaId = (int?)null,
                    numeroFactura = (string)null,
                    total = total,
                    cambio = request.MontoPagado - total,
                    estado = "PendienteSync",
                    idLocal = idLocal
                };
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(response));
            }
        }

        // 5. Pagar factura de reparación
        [HttpPost("facturas/{id}/pago")]
        public async Task<IActionResult> PagarFactura(int id, [FromBody] PagoFacturaRequest request)
        {
            // Forzamos el id de ruta para evitar discrepancias entre URL y body.
            var requestCore = request with { FacturaId = id };

            try
            {
                var resultado = await core.PagarFacturaAsync(requestCore, ObtenerToken());
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch
            {
                var idLocal = await cola.EncolarOperacionAsync("Caja", "pago_factura", requestCore);
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(new
                {
                    facturaId = id,
                    estado = "PendienteSync",
                    idLocal = idLocal
                }));
            }
        }

        // 6. Registrar abono a CxC
        [HttpPost("cxc/{id}/abono")]
        public async Task<IActionResult> AbonoCxC(int id, [FromBody] AbonoCxCRequest request)
        {
            var resultado = await core.RegistrarAbonoAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 7. Sincronizar transacciones offline
        [HttpPost("sync")]
        public async Task<IActionResult> SyncOffline([FromBody] SyncRequest request)
        {
            var resultado = await cola.ProcesarLoteOfflineAsync(request);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // --- MÉTODO CORREGIDO ---
        [HttpGet("facturas/{clienteId}")]
        public async Task<IActionResult> ObtenerFacturasCliente(int clienteId)
        {
            try
            {
                // Cambiado '_core' por 'core' para que coincida con el constructor
                var facturas = await core.ObtenerFacturasPorClienteAsync(clienteId);

                if (facturas == null)
                {
                    return NotFound(FrenosIntegracion.Helpers.ApiResponse<object>.Fail("NOT_FOUND", "No se encontraron facturas."));
                }

                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(facturas));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail("CORE_UNAVAILABLE", ex.Message));
            }
        }
        [HttpGet("facturas/buscar")]
        public async Task<IActionResult> BuscarFacturas([FromQuery] string? numeroFactura, [FromQuery] string? placa)
        {
            try
            {
                var facturas = await core.ObtenerFacturasPendientesAsync(ObtenerToken(), numeroFactura, placa);
                if (facturas == null)
                {
                    return NotFound(FrenosIntegracion.Helpers.ApiResponse<object>.Fail("NOT_FOUND", "No se encontraron facturas que coincidan con la búsqueda."));
                }
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(facturas));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail("CORE_UNAVAILABLE", ex.Message));
            }
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }

        // GET: int/caja/ordenes/{id}/estado
        [HttpGet("ordenes/{id}/estado")]
        public async Task<IActionResult> ObtenerEstadoOrden(int id)
        {
            try
            {
                // Este método YA existe en tu ICoreService según lo que me mostraste antes
                var resultado = await core.ObtenerEstadoOrdenAsync(id, ObtenerToken());
                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail("CORE_UNAVAILABLE", ex.Message));
            }
        }

        [HttpPost("venta-directa")]
        public async Task<IActionResult> VentaDirecta([FromBody] object request)
        {
            try
            {
                var token = ObtenerToken();

                var cobroRequest = System.Text.Json.JsonSerializer
                    .Deserialize<CobroRequest>(
                        System.Text.Json.JsonSerializer.Serialize(request),
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (cobroRequest == null)
                    return BadRequest(FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                        "VALIDATION_ERROR", "Request inválido."));

                var resultado = await core.ProcesarCobroAsync(cobroRequest, token);

                return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
            }
            catch (Exception ex)
            {
                return StatusCode(503, FrenosIntegracion.Helpers.ApiResponse<object>.Fail(
                    "CORE_ERROR", ex.Message));
            }
        }
    }
}