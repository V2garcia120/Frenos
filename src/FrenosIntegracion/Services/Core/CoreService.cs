using FrenosIntegracion.DTOs;
using FrenosIntegracion.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FrenosIntegracion.Services.Core
{
    public class CoreService : ICoreService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // Constructor tradicional para evitar el error CS8863
        public CoreService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> EstaDisponibleAsync()
        {
            try { var r = await _http.GetAsync("/health"); return r.IsSuccessStatusCode; }
            catch (Exception ex) {
                Console.WriteLine($"Error al verificar disponibilidad del Core: {ex.Message}");
                return false;
            }
        }

        // --- Autenticación ---
        public async Task<object> AutenticarClienteAsync(LoginRequest request)
        {
            var response = await _http.PostAsync("/api/auth/login-cliente", Serializar(request));
            response.EnsureSuccessStatusCode();

            var wrapper = JsonSerializer.Deserialize<ApiWrapper<TokenDto>>(
                await response.Content.ReadAsStringAsync(), _jsonOptions);

            return new
            {
                token = wrapper?.Data?.Token ?? "",
                email = request.Email,        
                rol = "Cliente",
                clienteId = 0                   
            };
        }

        public async Task<object> AutenticarEmpleadoAsync(LoginRequest request)
        {
            var response = await _http.PostAsync("/api/auth/empleado", Serializar(request));
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }
        public async Task<object> AutenticarCajeroAsync(LoginRequest request)
        {
            var response = await _http.PostAsync("/api/auth/login-cajero", Serializar(request));
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }
     
        // --- Gestión de Órdenes Web (Pág. 7-9) ---
        // Estos faltaban y causaban el error CS0535
        public async Task<OrdenWebResponse> CrearOrdenAsync(CrearOrdenWebRequest request, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsync("/api/ordenes", Serializar(request));
            response.EnsureSuccessStatusCode();
            return (await Deserializar<OrdenWebResponse>(response))!;
        }

        public async Task<EstadoOrdenResponse> ObtenerEstadoOrdenAsync(int ordenId, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync($"/api/ordenes/{ordenId}/estado");
            response.EnsureSuccessStatusCode();
            return (await Deserializar<EstadoOrdenResponse>(response))!;
        }

        // --- Gestión de Turnos y Caja ---
        public async Task<object> AbrirTurnoAsync(int cajeroId, decimal montoInicial)
        {
            var payload = new { cajeroId, montoInicial };
            var response = await _http.PostAsync("/api/caja/abrir", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<object> CerrarTurnoAsync(int turnoId, decimal efectivoContado)
        {
            var payload = new { turnoId, efectivoContado };
            var response = await _http.PostAsync("/api/caja/cerrar", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<object> RegistrarMovimientoEfectivoAsync(int turnoId, decimal monto, string motivo, string tipo)
        {
            var payload = new { turnoId, monto, motivo, tipo };
            var response = await _http.PostAsync("/api/caja/movimiento", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        // --- Pagos ---
        public async Task<object> PagarFacturaAsync(int facturaId, int turnoId, string metodo, decimal monto)
        {
            var payload = new { facturaId, turnoId, metodo, monto };
            var response = await _http.PostAsync($"/api/facturas/{facturaId}/pago", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<object> RegistrarAbonoAsync(int cxcId, int turnoId, decimal monto, string metodo)
        {
            var payload = new { cxcId, turnoId, monto, metodo };
            var response = await _http.PostAsync($"/api/cxc/{cxcId}/abono", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        // --- Catálogos ---


        public async Task<CobroResponse> ProcesarCobroAsync(CobroRequest request, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsync("/api/facturas", Serializar(request));
            response.EnsureSuccessStatusCode();
            return (await Deserializar<CobroResponse>(response))!;
        }

        // --- Helpers ---
        private static StringContent Serializar<T>(T obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

        private async Task<T?> Deserializar<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ApiWrapper<T>>(json, _jsonOptions);
            return wrapper != null ? wrapper.Data : default;
        }

        public async Task<IEnumerable<object>> ObtenerFacturasPorClienteAsync(int clienteId)
        {
            // 1. Hacemos la petición al puerto 7001 (Core)
            // El Core debería tener un endpoint que filtre por clienteId
            var response = await _http.GetAsync($"/api/facturas/cliente/{clienteId}");

            // 2. Si no es exitoso (404 o 500), devolvemos lista vacía
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<object>();

            // 3. Deserializamos usando el helper que ya tienes en la clase
            var data = await Deserializar<IEnumerable<object>>(response);

            return data ?? Enumerable.Empty<object>();
        }

        // --- En CoreService.cs ---

        public async Task<IEnumerable<object>> ObtenerVehiculosClienteAsync()
        {
            var response = await _http.GetAsync("/api/vehiculos");
            return await Deserializar<IEnumerable<object>>(response) ?? Enumerable.Empty<object>();
        }

        public async Task<object> RegistrarVehiculoAsync(object vehiculo)
        {
            var response = await _http.PostAsync("/api/vehiculos", Serializar(vehiculo));
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<IEnumerable<object>> ObtenerHistorialOrdenesAsync()
        {
            var response = await _http.GetAsync("/api/ordenes/historial");
            return await Deserializar<IEnumerable<object>>(response) ?? Enumerable.Empty<object>();
        }

        public async Task<bool> RegistrarClienteAsync(ClienteRegistroDto cliente)
        {
            var payload = new
            {
                Nombre = $"{cliente.Nombre} {cliente.Apellido}".Trim(),
                Cedula = cliente.Cedula,
                Telefono = cliente.Telefono,
                Email = cliente.Correo,
                Password = cliente.Password,
                Direccion = ""
            };

            var response = await _http.PostAsJsonAsync("api/clientes", payload);

            if (response.IsSuccessStatusCode)
                return true;

            var json = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ApiErrorWrapper>(json, _jsonOptions);
            var mensaje = error?.Error?.Mensaje ?? "No se pudo registrar el cliente.";
            throw new InvalidOperationException(mensaje);
        }

        // Token interno que Integración usa para llamar al Core
        private string GenerarTokenInterno()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("TallerFrenosClaveSecreta2026!MuyLargaParaHMAC256"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TallerCore",
                claims: new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "0"),
            new Claim(ClaimTypes.Email, "integracion@sistema.interno"),
            new Claim(ClaimTypes.Role, "Admin")
                },
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // --- Catálogos ---
        public async Task<IEnumerable<ProductoDto>> ObtenerProductosAsync()
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.GetAsync("/api/productos");
            var data = await Deserializar<IEnumerable<ProductoDto>>(response);
            return data ?? Enumerable.Empty<ProductoDto>();
        }

        public async Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync()
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.GetAsync("/api/servicios");
            var data = await Deserializar<IEnumerable<ServicioDto>>(response);
            return data ?? Enumerable.Empty<ServicioDto>();
        }
    }


    internal record ApiErrorDto(string Codigo, string Mensaje);
    internal record ApiErrorWrapper(bool Success, object? Data, ApiErrorDto? Error);
    internal record ApiWrapper<T>(bool Success, T? Data, object? Error);
    internal record TokenDto(string Token);
}