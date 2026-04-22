using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Services.Sync;
using FrenosIntegracion.Helpers; // Asegúrate de que ApiResponse esté aquí
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
            var resultado = await core.AbrirTurnoAsync(request.CajeroId, request.MontoInicial);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 2. Cerrar turno de caja
        [HttpPost("turno/cerrar")]
        public async Task<IActionResult> CerrarTurno([FromBody] CerrarTurnoRequest request)
        {
            var resultado = await core.CerrarTurnoAsync(request.TurnoId, request.EfectivoContado);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 3. Entrada y salida de efectivo
        [HttpPost("efectivo/entrada")]
        public async Task<IActionResult> EntradaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request.TurnoId, request.Monto, request.Motivo, "Entrada");
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        [HttpPost("efectivo/salida")]
        public async Task<IActionResult> SalidaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request.TurnoId, request.Monto, request.Motivo, "Salida");
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
                var idLocal = Guid.NewGuid().ToString();
                await cola.EncolarOperacionAsync("Caja", "cobro", request);

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
            // Si el pago viene de la Web y no trae turno, le asignamos el ID 999 (por ejemplo)
            int turnoParaElCore = request.TurnoId == 0 ? 999 : request.TurnoId;

            var resultado = await core.PagarFacturaAsync(id, turnoParaElCore, request.MetodoPago, request.Monto);
            return Ok(FrenosIntegracion.Helpers.ApiResponse<object>.Ok(resultado));
        }

        // 6. Registrar abono a CxC
        [HttpPost("cxc/{id}/abono")]
        public async Task<IActionResult> AbonoCxC(int id, [FromBody] AbonoCxCRequest request)
        {
            var resultado = await core.RegistrarAbonoAsync(id, request.TurnoId, request.Monto, request.MetodoPago);
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
    }
}