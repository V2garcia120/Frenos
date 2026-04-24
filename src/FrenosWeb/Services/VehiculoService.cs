using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class VehiculoService
    {
        private readonly HttpClient _http;

        public VehiculoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<VehiculoModel>> GetVehiculosUsuarioAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<ApiResponse<List<VehiculoModel>>>("int/vehiculos/mis-vehiculos");

                if (response != null && response.Success && response.Data != null)
                {
                    return response.Data;
                }

                return ObtenerVehiculosPrueba();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] API Vehículos no disponible: {ex.Message}");
                return ObtenerVehiculosPrueba();
            }
        }

        public async Task<bool> RegistrarVehiculoAsync(VehiculoModel vehiculo)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("int/vehiculos", vehiculo);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    return result?.Success ?? false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error al registrar vehículo: {ex.Message}");
                return false;
            }
        }

        private List<VehiculoModel> ObtenerVehiculosPrueba()
        {
            return new List<VehiculoModel>
            {
                new VehiculoModel { Id = 1, Marca = "Toyota", Modelo = "Corolla", Placa = "A987456" },
                new VehiculoModel { Id = 2, Marca = "Honda", Modelo = "Civic", Placa = "G123456" }
            };
        }
    }
}