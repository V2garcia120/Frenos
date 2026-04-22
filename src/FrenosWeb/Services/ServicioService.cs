using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class ServicioService
    {
        private readonly HttpClient _http;
        public ServicioService(HttpClient http) => _http = http;

        public async Task<List<Servicio>> GetServiciosAsync()
        {
            try
            {
                var resp = await _http.GetFromJsonAsync<List<Servicio>>("api/servicios");
                return resp ?? new();
            }
            catch
            {
                return new List<Servicio>
                {
                    new Servicio { Id = 101, Nombre = "Cambio de Pastillas", Precio = 1200, RequiereVehiculo = true },
                    new Servicio { Id = 102, Nombre = "Rectificación de Discos", Precio = 2000, RequiereVehiculo = true },
                    new Servicio { Id = 103, Nombre = "Mantenimiento General", Precio = 5000, RequiereVehiculo = true }
                };
            }
        }

        public async Task<List<Servicio>> GetProductosDelInventarioAsync()
        {
            try
            {
                var resp = await _http.GetFromJsonAsync<List<Servicio>>("api/productos");
                return resp ?? new();
            }
            catch
            {
                return new List<Servicio>
                {
                    new Servicio { Id = 1, Nombre = "Pastillas Cerámicas", Precio = 2500, RequiereVehiculo = false },
                    new Servicio { Id = 2, Nombre = "Discos de Freno", Precio = 4000, RequiereVehiculo = false },
                    new Servicio { Id = 3, Nombre = "Líquido de Frenos DOT4", Precio = 800, RequiereVehiculo = false }
                };
            }
        }
    }
}