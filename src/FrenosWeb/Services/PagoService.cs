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
                var response = await _http.PostAsJsonAsync("int/caja/cobro", request);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResponse<CobroResponse>>();
                    return resultado ?? ApiResponse<CobroResponse>.Fail("PARSE_ERROR", "Error al leer la respuesta del servidor.");
                }
                else
                {
                    return ApiResponse<CobroResponse>.Fail("SERVER_ERROR", $"Error de comunicación: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Error] Fallo en la conexión de pago: {ex.Message}");
                return ApiResponse<CobroResponse>.Fail("CONNECTION_ERROR", "No se pudo establecer conexión con el portal de pagos.");
            }
        }
    }
}