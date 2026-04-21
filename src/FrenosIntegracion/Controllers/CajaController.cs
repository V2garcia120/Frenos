using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Services.Sync;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FrenosIntegracion.Controllers
{
    [ApiController]
    [Route("int/caja")]
    [Authorize]
    public class CajaController(ICoreService core, IColaSyncService cola) : ControllerBase
    {
        // 1. Abrir turno de caja (Pág. 10)
        [HttpPost("turno/abrir")]
        public async Task<IActionResult> AbrirTurno([FromBody] AbrirTurnoRequest request)
        {
            var resultado = await core.AbrirTurnoAsync(request.CajeroId, request.MontoInicial);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        // 2. Cerrar turno de caja (Pág. 10)
        [HttpPost("turno/cerrar")]
        public async Task<IActionResult> CerrarTurno([FromBody] CerrarTurnoRequest request)
        {
            var resultado = await core.CerrarTurnoAsync(request.TurnoId, request.EfectivoContado);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        // 3. Entrada y salida de efectivo (Pág. 11) 
        [HttpPost("efectivo/entrada")]
        public async Task<IActionResult> EntradaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request.TurnoId, request.Monto, request.Motivo, "Entrada");
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        [HttpPost("efectivo/salida")]
        public async Task<IActionResult> SalidaEfectivo([FromBody] MovimientoEfectivoRequest request)
        {
            var resultado = await core.RegistrarMovimientoEfectivoAsync(request.TurnoId, request.Monto, request.Motivo, "Salida");
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        // 4. Procesar cobro de venta directa (Pág. 11)
        [HttpPost("cobro")]
        public async Task<IActionResult> Cobro([FromBody] CobroRequest request)
        {
            try
            {
                var resultado = await core.ProcesarCobroAsync(request, ObtenerToken());
                return Ok(ApiResponse<object>.Ok(resultado));
            }
            catch
            {
                // Fallback offline: se genera un ID local y se encola
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
                return Ok(ApiResponse<object>.Ok(response));
            }
        }

        // 5. Pagar factura de reparación (Pág. 12)
        [HttpPost("facturas/{id}/pago")]
        public async Task<IActionResult> PagarFactura(int id, [FromBody] PagoFacturaRequest request)
        {
            // Nota: Asegúrate que este método acepte los parámetros según tu ICoreService
            var resultado = await core.PagarFacturaAsync(id, request.TurnoId, request.MetodoPago, request.Monto);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        // 6. Registrar abono a CxC (Pág. 12)
        [HttpPost("cxc/{id}/abono")]
        public async Task<IActionResult> AbonoCxC(int id, [FromBody] AbonoCxCRequest request)
        {
            var resultado = await core.RegistrarAbonoAsync(id, request.TurnoId, request.Monto, request.MetodoPago);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        // 7. Sincronizar transacciones offline (Pág. 13) 
        [HttpPost("sync")]
        public async Task<IActionResult> SyncOffline([FromBody] SyncRequest request)
        {
            var resultado = await cola.ProcesarLoteOfflineAsync(request);
            return Ok(ApiResponse<object>.Ok(resultado));
        }

        private string ObtenerToken()
        {
            var auth = Request.Headers.Authorization.ToString();
            return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
        }
    }
}