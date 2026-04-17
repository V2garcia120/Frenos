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

        public async Task<bool> CrearOrdenAsync(List<CarritoItem> items)
        {
            // Aquí es donde en el futuro haremos:
            // var response = await _http.PostAsJsonAsync("api/ordenes", items);

            // Por ahora, simulamos que la API respondió que todo está OK (Ciberseguridad: Integridad)
            await Task.Delay(1000);
            return true;
        }
    }
}