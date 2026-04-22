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
                // Intento real a la API
                var response = await _http.GetFromJsonAsync<List<VehiculoModel>>("api/vehiculos/mis-vehiculos");
                return response ?? new List<VehiculoModel>();
            }
            catch (Exception ex)
            {
                // Log para auditoria en consola
                Console.WriteLine($"[Cyber-Logs] API Vehículos no disponible: {ex.Message}");

                // Importante que coincidan con los IDs que usas en el Carrito
                return new List<VehiculoModel>
        {
            new VehiculoModel { Id = 1, Marca = "Toyota", Modelo = "Corolla", Placa = "A987456" },
            new VehiculoModel { Id = 2, Marca = "Honda", Modelo = "Civic", Placa = "G123456" }
        };
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