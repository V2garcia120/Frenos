using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                // Intentamos conectar con la API de Integracion 
                var respuesta = await _http.GetFromJsonAsync<List<Producto>>("api/productos");
                return respuesta ?? new List<Producto>();
            }
            catch (Exception ex)
            {
                // Si la API falla cargamos los datos simulados
                Console.WriteLine($"[Cyber-Logs] API no disponible: {ex.Message}");

                return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Pastillas de Freno Cerámicas", Precio = 2500, ImagenUrl = "frenos.jpg" },
                new Producto { Id = 2, Nombre = "Disco de Freno Delantero", Precio = 4500, ImagenUrl = "disco.jpg" }
            };
            }
        }
    }
}
