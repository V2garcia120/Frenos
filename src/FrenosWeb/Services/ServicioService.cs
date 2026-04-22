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
                var resp = await _http.GetFromJsonAsync<ApiResponse<CatalogoResponse>>("int/catalogo/buscar?q=");

                if (resp != null && resp.Success && resp.Data != null)
                {
                    return resp.Data.Servicios;
                }
                return ObtenerServiciosPrueba();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error en Catálogo: {ex.Message}");
                return ObtenerServiciosPrueba();
            }
        }

        public async Task<List<Servicio>> GetProductosDelInventarioAsync()
        {
            try
            {
                var resp = await _http.GetFromJsonAsync<ApiResponse<List<Servicio>>>("int/catalogo/productos");

                if (resp != null && resp.Success && resp.Data != null)
                {
                    return resp.Data;
                }
                return ObtenerProductosPrueba();
            }
            catch
            {
                return ObtenerProductosPrueba();
            }
        }

        private List<Servicio> ObtenerServiciosPrueba() => new()
        {
            new Servicio { Id = 101, Nombre = "Cambio de Pastillas", Precio = 1200, RequiereVehiculo = true },
            new Servicio { Id = 102, Nombre = "Rectificación de Discos", Precio = 2000, RequiereVehiculo = true },
            new Servicio { Id = 103, Nombre = "Mantenimiento General", Precio = 5000, RequiereVehiculo = true }
        };

        private List<Servicio> ObtenerProductosPrueba() => new()
        {
            new Servicio { Id = 1, Nombre = "Pastillas Cerámicas", Precio = 2500, RequiereVehiculo = false },
            new Servicio { Id = 2, Nombre = "Discos de Freno", Precio = 4000, RequiereVehiculo = false },
            new Servicio { Id = 3, Nombre = "Líquido de Frenos DOT4", Precio = 800, RequiereVehiculo = false }
        };
    }

    public class CatalogoResponse
    {
        public List<Servicio> Productos { get; set; } = new();
        public List<Servicio> Servicios { get; set; } = new();
    }
}