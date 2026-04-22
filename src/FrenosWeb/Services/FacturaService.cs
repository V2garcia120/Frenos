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

        public async Task<List<FacturaModel>> GetFacturasUsuarioAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<ApiResponse<List<FacturaModel>>>("int/facturas");
                if(response != null && response.Success && response.Data != null)
                {
                    return response.Data;
                }
                {
                    return ObtenerDatosPrueba();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error conectando a Integración: {ex.Message}");
                return ObtenerDatosPrueba();
            }
        }

        public void MarcarComoPagada(decimal monto)
        {
            // Lógica temporal para la demo: 
            // Esto solo funcionaría si tuviéramos la lista en memoria.
            // Por ahora, está puesto para que el compilador no de error.
            Console.WriteLine($"[Cyber-Logs] Factura de RD$ {monto} marcada como pagada localmente.");
            // Por ahora solo logueamos, cuando Diana tenga el POST de pago lo conectamos aquí
        }

        private List<FacturaModel> ObtenerDatosPrueba()
        {
            return new List<FacturaModel>
            {
                new FacturaModel { NumeroFactura = "FAC-2026-001", ServicioRealizado = "Cambio Pastillas Cerámicas", Total = 4500.00m, EstadoPago = "Pagado", Fecha = DateTime.Now.AddDays(-10) },
                new FacturaModel { NumeroFactura = "FAC-2026-002", ServicioRealizado = "Rectificación y Líquido", Total = 3200.00m, EstadoPago = "Pendiente", Fecha = DateTime.Now }
            };
        }
    }
}