using FrenosWeb.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace FrenosWeb.Services
{
    public class VehiculoService
    {
        private readonly HttpClient _http;

        public VehiculoService(HttpClient http)
        {
            _http = http;
        }

        // Lee el clienteId del JWT guardado en el header del HttpClient
        private int ObtenerClienteIdDelToken()
        {
            try
            {
                var authHeader = _http.DefaultRequestHeaders.Authorization?.Parameter;
                if (string.IsNullOrEmpty(authHeader)) return 0;

                var payload = authHeader.Split('.')[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                // El Core genera el token con "sub" = clienteId
                if (claims != null && claims.TryGetValue("sub", out var sub))
                    return int.TryParse(sub.ToString(), out var id) ? id : 0;
            }
            catch { }
            return 0;
        }

        public async Task<List<VehiculoModel>> GetVehiculosUsuarioAsync()
        {
            try
            {
                var clienteId = ObtenerClienteIdDelToken();
                if (clienteId == 0)
                {
                    Console.WriteLine("[VehiculoService] No se pudo obtener clienteId del token.");
                    return new List<VehiculoModel>();
                }

                var response = await _http.GetFromJsonAsync<ApiResponse<List<VehiculoModel>>>(
                    $"int/vehiculos/mis-vehiculos?clienteId={clienteId}");

                if (response != null && response.Success && response.Data != null)
                    return response.Data;

                return new List<VehiculoModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al obtener vehículos: {ex.Message}");
                return new List<VehiculoModel>();
            }
        }

        public async Task<(bool Exito, string Mensaje)> RegistrarVehiculoAsync(VehiculoModel vehiculo)
        {
            try
            {
                var clienteId = ObtenerClienteIdDelToken();
                if (clienteId == 0) return (false, "No se pudo obtener la sesión.");

                var response = await _http.PostAsJsonAsync("int/vehiculos", new
                {
                    ClienteId = clienteId,
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Anno = vehiculo.Anio,
                    Color = vehiculo.Color,
                    VIN = (string?)null,
                    TipoCombustible = (string?)null,
                    KmActual = 0,
                    Nota = (string?)null
                });

                if (response.IsSuccessStatusCode)
                    return (true, "");

                //Lee el mensaje real del servidor
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<object>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (false, result?.Error?.Mensaje ?? "No se pudo registrar el vehículo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al registrar vehículo: {ex.Message}");
                return (false, ex.Message);
            }
        }

        public async Task<bool> ActualizarVehiculoAsync(int id, VehiculoModel vehiculo)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"int/vehiculos/{id}", new
                {
                    Placa = vehiculo.Placa,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Anno = vehiculo.Anio,
                    Color = vehiculo.Color,
                    VIN = (string?)null,
                    TipoCombustible = (string?)null,
                    KmActual = 0,
                    Nota = (string?)null
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al actualizar vehículo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarVehiculoAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"int/vehiculos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VehiculoService] Error al eliminar vehículo: {ex.Message}");
                return false;
            }
        }
    }
}