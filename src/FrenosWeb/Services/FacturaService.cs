using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class FacturaService(HttpClient http)
    {
        public async Task<List<FacturaModel>> GetFacturasAsync()
        {
            try
            {
                var response = await http.GetFromJsonAsync<ApiResponse<List<FacturaModel>>>(
                    "int/facturas/mis-facturas",
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return response?.Success == true && response.Data != null
                    ? response.Data
                    : [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error al cargar facturas: {ex.Message}");
                return [];
            }
        }

        public async Task<bool> ProcesarPagoFacturaAsync(int facturaId, decimal monto, int turnoId = 0)
        {
            try
            {
                var request = new { TurnoId = turnoId, MetodoPago = "Tarjeta", Monto = monto };
                var response = await http.PostAsJsonAsync($"int/caja/facturas/{facturaId}/pago", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    return result?.Success ?? false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error al procesar pago: {ex.Message}");
                return false;
            }
        }
    }
}
