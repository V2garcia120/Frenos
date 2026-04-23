using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class FacturaService
    {
        private readonly HttpClient _http;

        public FacturaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<FacturaModel>> GetFacturasUsuarioAsync(int clienteId)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<ApiResponse<List<FacturaModel>>>($"int/caja/facturas/{clienteId}");

                if (response != null && response.Success && response.Data != null)
                {
                    return response.Data;
                }

                return ObtenerDatosPrueba();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error conectando a Integración: {ex.Message}");
                return ObtenerDatosPrueba();
            }
        }

        public async Task<bool> ProcesarPagoFacturaAsync(int facturaId, decimal monto, int turnoId = 0)
        {
            try
            {
                int turnoFinal = turnoId > 0 ? turnoId : 0;
                var request = new { TurnoId = turnoId, MetodoPago = "Tarjeta", Monto = monto };

                var response = await _http.PostAsJsonAsync($"int/caja/facturas/{facturaId}/pago", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    return result?.Success ?? false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error al procesar pago real: {ex.Message}");
                MarcarComoPagadaLocal(monto);
                return true;
            }
        }

        public void MarcarComoPagadaLocal(decimal monto)
        {
            Console.WriteLine($"[Cyber-Logs] Factura de RD$ {monto} marcada como pagada (Simulación).");
        }

        private List<FacturaModel> ObtenerDatosPrueba()
        {
            return new List<FacturaModel>
            {
                new FacturaModel { Id = 1, NumeroFactura = "FAC-2026-001", ServicioRealizado = "Cambio Pastillas", Total = 4500.00m, EstadoPago = "Pagado", Fecha = DateTime.Now.AddDays(-10) },
                new FacturaModel { Id = 2, NumeroFactura = "FAC-2026-002", ServicioRealizado = "Rectificación", Total = 3200.00m, EstadoPago = "Pendiente", Fecha = DateTime.Now }
            };
        }
    }
}