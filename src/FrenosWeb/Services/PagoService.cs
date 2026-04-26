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

                var tieneVehiculo = request.VehiculoId.HasValue && request.VehiculoId > 0;
                var servicios = tieneVehiculo ? items.ToList() : new List<CobroItemRequest>();
                var productos = tieneVehiculo ? new List<CobroItemRequest>() : items.ToList();

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
                        notas = $"{metodoPago} | Items: {string.Join(", ", servicios.Select(i => i.Nombre))}"
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
                            montoPagado = productos.Sum(i => i.PrecioSnapshot * i.Cantidad) * 1.18m,
                            items = productos.Select(i => new
                            {
                                tipo = i.Tipo,
                                itemId = i.ItemId,
                                cantidad = i.Cantidad,
                                precioSnapshot = i.PrecioSnapshot
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
                            notas = $"Efectivo | Items: {string.Join(", ", productos.Select(i => i.Nombre))} | Total: RD${productos.Sum(i => i.PrecioSnapshot * i.Cantidad):N2}"
                        };

                        var resp = await _http.PostAsJsonAsync("int/ordenes/web", ordenRequest);

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