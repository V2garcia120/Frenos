using Azure.Core;
using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using FrenosIntegracion.Services.Sync;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("int/caja")]
[Authorize]
public class CajaController(ICoreService core, IColaSyncService cola) : ControllerBase
{
    // POST int/caja/cobro
    [HttpPost("cobro")]
    public async Task<IActionResult> Cobro([FromBody] CobroRequest request)
    {
        var token = ObtenerToken();
        try
        {
            var resultado = await core.ProcesarCobroAsync(request, token);
            return Ok(ApiResponse<object>.Ok(resultado));
        }
        catch
        {
            // Core no disponible — encolar para sincronizar después
            var idLocal = Guid.NewGuid().ToString();
            var payload = JsonSerializer.Serialize(request);
            await cola.EncolarOperacionAsync("Caja", "cobro", request);

            return Ok(ApiResponse<object>.Ok(new CobroResponse(
                FacturaId: null,
                NumeroFactura: null,
                Total: request.Items.Sum(i => i.Cantidad * i.PrecioSnapshot),
                Cambio: request.MontoPagado -
                               request.Items.Sum(i => i.Cantidad * i.PrecioSnapshot),
                Estado: "PendienteSync",
                IdLocal: idLocal)));
        }
    }

    private string ObtenerToken()
    {
        var auth = Request.Headers.Authorization.ToString();
        return auth.StartsWith("Bearer ") ? auth[7..] : string.Empty;
    }
}
