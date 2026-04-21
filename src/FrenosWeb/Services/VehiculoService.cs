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

        // Obtener la lista de vehículos desde la API
        public async Task<List<VehiculoModel>> GetVehiculosUsuarioAsync()
        {
            try
            {
                // Cuando tu API esté lista, esta será la ruta:
                var response = await _http.GetFromJsonAsync<List<VehiculoModel>>("api/vehiculos/mis-vehiculos");
                return response ?? new List<VehiculoModel>();
            }
            catch (Exception ex)
            {
                // Si la API falla (porque aún la estamos probando), 
                // imprimimos el error en la consola (F12) para ciber-auditoría
                Console.WriteLine($"[Cyber-Logs] Error de conexión: {ex.Message}");
                return new List<VehiculoModel>();
            }
        }

        // Registrar un nuevo vehículo
        public async Task<bool> RegistrarVehiculoAsync(VehiculoModel vehiculo)
        {
            var response = await _http.PostAsJsonAsync("api/vehiculos", vehiculo);
            return response.IsSuccessStatusCode;
        }
    }
}