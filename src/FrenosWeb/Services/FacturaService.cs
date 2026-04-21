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
                var response = await _http.GetFromJsonAsync<ApiResponse<List<FacturaModel>>>("api/integracion/facturas");
                return response?.Data ?? new List<FacturaModel>();
            }
            catch
            {
                // Si la API no está lista devolvemos datos de prueba para que tu parte no se rompa
                return new List<FacturaModel>
                {
                    new FacturaModel { NumeroFactura = "FAC-2026-001", ServicioRealizado = "Cambio Pastillas Cerámicas", Total = 4500.00m, EstadoPago = "Pagado", Fecha = DateTime.Now.AddDays(-10) },
                    new FacturaModel { NumeroFactura = "FAC-2026-002", ServicioRealizado = "Rectificación y Líquido", Total = 3200.00m, EstadoPago = "Pendiente", Fecha = DateTime.Now }
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public object? Error { get; set; }
    }
}