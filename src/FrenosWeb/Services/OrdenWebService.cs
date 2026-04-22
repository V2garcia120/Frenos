using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class OrdenWebService
    {
        private readonly HttpClient _http;

        public OrdenWebService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ApiResponse<CobroResponse>?> ProcesarCobroOrdenAsync(CobroRequest request)
        {
            try
            {
                // Llamamos al endpoint de Integración: POST int/caja/cobro
                var response = await _http.PostAsJsonAsync("int/caja/cobro", request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<CobroResponse>>();
                }

                return new ApiResponse<CobroResponse> { Success = false, Message = "Error en el servidor de integración." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Fallo de conexión: {ex.Message}");
                return null;
            }
        }

        // Método para consultar el historial (Para MisOrdenes.razor)
        public async Task<List<OrdenWebModel>> GetHistorialAsync()
        {
            try
            {
                // Integración aún no nos ha pasado este endpoint, pero siguiendo su lógica sería así:
                var response = await _http.GetFromJsonAsync<ApiResponse<List<OrdenWebModel>>>("int/ordenes/historial");
                return response?.Data ?? ObtenerDatosPrueba();
            }
            catch
            {
                return ObtenerDatosPrueba();
            }
        }

        private List<OrdenWebModel> ObtenerDatosPrueba()
        {
            return new List<OrdenWebModel>
            {
                new OrdenWebModel { NumeroOrden = "ORD-2026-001", Fecha = DateTime.Now.AddDays(-2), Vehiculo = "Toyota Corolla 2022", Placa = "A987456", Total = 12390.00m, EstadoServicio = "Listo", EsServicio = true, TipoServicio = "Mantenimiento" },
                new OrdenWebModel { NumeroOrden = "ORD-2026-002", Fecha = DateTime.Now, Vehiculo = "Honda Civic 2018", Placa = "G123456", Total = 4500.00m, EstadoServicio = "En diagnóstico", EsServicio = true, TipoServicio = "Frenos" }
            };
        }
    }
}