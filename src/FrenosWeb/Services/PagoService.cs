using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class PagoService
    {
        private readonly HttpClient _http;

        public PagoService(HttpClient http) => _http = http;

        public async Task<ApiResponse<CobroResponse>> ProcesarCobroAsync(
            CobroRequest request, string metodoPago)
        {
            try
            {
                if (_http.DefaultRequestHeaders.Authorization == null)
                    return ApiResponse<CobroResponse>.Fail(
                        "AUTH_ERROR", "No tienes una sesión activa.");

                var items = request.Items ?? new();

                var servicios = items.Where(i => i.Tipo == "Servicio").ToList();
                var productos = items.Where(i => i.Tipo != "Servicio").ToList(); 

                // ── SERVICIOS → siempre quedan pendientes como Orden ──
                if (servicios.Any())
                {
                    var ordenRequest = new
                    {
                        clienteId = request.ClienteId,
                        vehiculoId = request.VehiculoId,
                        items = servicios.Select(i => new
                        {
                            tipo = i.Tipo,
                            itemId = i.ItemId,
                            cantidad = i.Cantidad,
                            precioSnapshot = i.PrecioSnapshot
                        }),
                        notas = $"Orden web - Pago: {metodoPago}"
                    };

                    var resp = await _http.PostAsJsonAsync("int/ordenes/web", ordenRequest);
                    if (!resp.IsSuccessStatusCode)
                    {
                        var errJson = await resp.Content.ReadAsStringAsync();
                        Console.WriteLine($"[PagoService] Error al crear orden: {resp.StatusCode} - {errJson}");

                        try
                        {
                            var errObj = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<object>>(
                                errJson, new System.Text.Json.JsonSerializerOptions
                                { PropertyNameCaseInsensitive = true });
                            return ApiResponse<CobroResponse>.Fail(
                                "ORDER_ERROR", errObj?.Error?.Mensaje ?? $"Error {resp.StatusCode}");
                        }
                        catch
                        {
                            return ApiResponse<CobroResponse>.Fail(
                                "ORDER_ERROR", $"Error al crear la orden ({resp.StatusCode})");
                        }
                    }
                }

                if (productos.Any())
                {
                    if (metodoPago == "Tarjeta" || metodoPago == "Transferencia")
                    {
                        var ventaRequest = new
                        {
                            clienteId = request.ClienteId,
                            metodoPago = metodoPago,
                            items = productos.Select(i => new
                            {
                                tipo = i.Tipo,
                                itemId = i.ItemId,
                                cantidad = i.Cantidad
                            })
                        };

                        var resp = await _http.PostAsJsonAsync(
                            "int/caja/venta-directa", ventaRequest);

                        if (!resp.IsSuccessStatusCode)
                            return ApiResponse<CobroResponse>.Fail(
                                "VENTA_ERROR", "No se pudo procesar la venta de productos.");

                        var resultado = await resp.Content
                            .ReadFromJsonAsync<ApiResponse<CobroResponse>>();
                        return resultado ?? ApiResponse<CobroResponse>.Fail(
                            "PARSE_ERROR", "Error al leer respuesta.");
                    }
                    else
                    {
                        // Efectivo → queda pendiente, el cliente paga al recoger
                        var ordenRequest = new
                        {
                            clienteId = request.ClienteId,
                            vehiculoId = (int?)null,
                            items = productos.Select(i => new
                            {
                                tipo = i.Tipo,
                                itemId = i.ItemId,
                                cantidad = i.Cantidad,
                                precioSnapshot = i.PrecioSnapshot
                            }),
                            notas = "Productos - Pago en efectivo al recoger"
                        };

                        var resp = await _http.PostAsJsonAsync(
                            "int/ordenes/web", ordenRequest);

                        if (!resp.IsSuccessStatusCode)
                            return ApiResponse<CobroResponse>.Fail(
                                "ORDER_ERROR", "No se pudo crear la orden de productos.");
                    }
                }

                return ApiResponse<CobroResponse>.Ok(new CobroResponse
                {
                    Estado = servicios.Any() ? "Pendiente" : "Pagada",
                    Total = request.MontoPagado
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Error] {ex.Message}");
                return ApiResponse<CobroResponse>.Fail(
                    "CONNECTION_ERROR", "No se pudo conectar con el servidor.");
            }
        }
    }
}