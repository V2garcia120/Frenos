using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class PagoService
    {
        private readonly HttpClient _http;

        public PagoService(HttpClient http)
        {
            _http = http;
        }
        public async Task<ApiResponse<CobroResponse>> ProcesarCobroAsync(CobroRequest request)
        {
            try
            {
                if (_http.DefaultRequestHeaders.Authorization == null)
                {
                    return ApiResponse<CobroResponse>.Fail("AUTH_ERROR", "No tienes una sesión activa para procesar pagos.");
                }

                var response = await _http.PostAsJsonAsync("int/caja/cobro", request);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResponse<CobroResponse>>();
                    return resultado ?? ApiResponse<CobroResponse>.Fail("PARSE_ERROR", "Error al leer la respuesta.");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ApiResponse<CobroResponse>.Fail("UNAUTHORIZED", "Tu sesión ha expirado. Por favor, inicia sesión de nuevo.");
                }

                return ApiResponse<CobroResponse>.Fail("SERVER_ERROR", $"La API respondió con código: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Error] Fallo crítico de red: {ex.Message}");
                return ApiResponse<CobroResponse>.Fail("CONNECTION_ERROR", "No se pudo conectar con el servidor de Integración.");
            }
        }
    }
}