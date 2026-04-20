using FrenosIntegracion.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FrenosIntegracion.Services.Core
{
    public class CoreService(HttpClient http) : ICoreService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<bool> EstaDisponibleAsync()
        {
            try { var r = await http.GetAsync("/health"); return r.IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerProductosAsync()
        {
            var response = await http.GetAsync("/api/productos");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var api = JsonSerializer.Deserialize<ApiWrapper<IEnumerable<ProductoDto>>>(json, _jsonOptions);
            return api?.Data ?? [];
        }

        public async Task<OrdenWebResponse> CrearOrdenAsync(
            CrearOrdenWebRequest request, string token)
        {
            var content = Serializar(request);
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await http.PostAsync("/api/ordenes", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var api = JsonSerializer.Deserialize<ApiWrapper<OrdenWebResponse>>(json, _jsonOptions);
            return api!.Data!;
        }

        public async Task<CobroResponse> ProcesarCobroAsync(
            CobroRequest request, string token)
        {
            var content = Serializar(request);
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await http.PostAsync("/api/facturas", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var api = JsonSerializer.Deserialize<ApiWrapper<CobroResponse>>(json, _jsonOptions);
            return api!.Data!;
        }

        public async Task<EstadoOrdenResponse> ObtenerEstadoOrdenAsync(int ordenId, string token)
        {
            // Configuramos el token de seguridad para la petición
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Hacemos la llamada al API del Core
            var response = await http.GetAsync($"/api/ordenes/{ordenId}/estado");

            // Si el Core responde con error (404, 500), esto lanzará una excepción que atrapará nuestro Controller
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Deserializamos la respuesta usando el formato estándar del proyecto
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<EstadoOrdenResponse>>(json, _jsonOptions);

            return apiResponse?.Data ?? throw new Exception("No se pudo obtener el estado de la orden.");
        }

        public async Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync()
        {
            // Hacemos la petición GET al endpoint de servicios
            var response = await http.GetAsync("/api/servicios");

            // Si hay error (ej. 401 o 500), lanzamos excepción
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Deserializamos usando la estructura estándar de tu API
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ServicioDto>>>(json, _jsonOptions);

            // Si viene nulo, devolvemos una lista vacía para evitar errores de null
            return apiResponse?.Data ?? Enumerable.Empty<ServicioDto>();
        }

        private static StringContent Serializar<T>(T obj)
        {
            // Usamos el constructor que acepta (string content, Encoding encoding, string mediaType)
            return new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json"
            );
        }
    }

    internal record ApiWrapper<T>(bool Success, T? Data, object? Error);

}
