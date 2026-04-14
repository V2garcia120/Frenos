using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class ServicioService
    {
        private readonly HttpClient _http;

        public ServicioService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Servicio>> GetServiciosAsync()
        {
            try
            {
                // Intentamos conectar con la API de Integracion
                var respuesta = await _http.GetFromJsonAsync<List<Servicio>>("api/servicios");
                return respuesta ?? new List<Servicio>();
            }
            catch (Exception ex)
            {
                // Si la API falla cargamos los datos simulados
                Console.WriteLine($"[Cyber-Logs] API Servicios no disponible: {ex.Message}");

                return new List<Servicio>
                {
                    new Servicio { Id = 1, Nombre = "Cambio de Pastillas", Precio = 1200 },
                    new Servicio { Id = 2, Nombre = "Rectificación de Discos", Precio = 3000 },
                    new Servicio { Id = 3, Nombre = "Mantenimiento Preventivo", Precio = 5000 }
                };
            }
        }
    }
}
