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

        public CoreService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> EstaDisponibleAsync()
        {
            try { var r = await _http.GetAsync("/health"); return r.IsSuccessStatusCode; }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar disponibilidad del Core: {ex.Message}");
                return false;
            }
        }

        // --- Autenticación ---
        public async Task<object> AutenticarClienteAsync(LoginRequest request)
        {
            // HttpRequestMessage evita condiciones de carrera en el header compartido
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login-cliente");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(request);

            var response = await _http.SendAsync(req);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Credenciales inválidas.");

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
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/auth/empleado");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(request);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<object> AutenticarCajeroAsync(LoginRequest request)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login-cajero");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(request);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }

        // --- Gestión de Órdenes Web ---
        public async Task<OrdenWebResponse> CrearOrdenAsync(CrearOrdenWebRequest request, string token)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/ordenes");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = Serializar(request);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return (await Deserializar<OrdenWebResponse>(response))!;
        }

        public async Task<EstadoOrdenResponse> ObtenerEstadoOrdenAsync(int ordenId, string token)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/api/ordenes/{ordenId}/estado");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return (await Deserializar<EstadoOrdenResponse>(response))!;
        }

        // --- Gestión de Turnos y Caja ---
        public async Task<object> AbrirTurnoAsync(int cajeroId, decimal montoInicial)
        {
            var payload = new { cajeroId, montoInicial };
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/caja/abrir");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(payload);

            var response = await _http.SendAsync(req);
            return await Deserializar<object>(response) ?? new { }; }
        public async Task<object> AbrirTurnoAsync(AbrirTurnoRequest request)
        {
            var response = await _http.PostAsync("/api/caja/turno/abrir", Serializar(request));
            var resultado = await DeserializarRespuestaOCuerpo<AbrirTurnoCoreResponse>(response);
            return resultado is not null ? (object)resultado : new { };
        }

        public async Task<object> CerrarTurnoAsync(CerrarTurnoRequest request)
        {
            var response = await _http.PostAsync("/api/caja/turno/cerrar", Serializar(request));
            var resultado = await DeserializarRespuestaOCuerpo<CerrarTurnoCoreResponse>(response);
            return resultado is not null ? (object)resultado : new { };
        }

        public async Task<object> RegistrarMovimientoEfectivoAsync(MovimientoEfectivoRequest request)
        {
            var response = await _http.PostAsync("/api/caja/movimiento", Serializar(request));
            return await Deserializar<object>(response) ?? new { };
        }

        // --- Pagos ---
        public async Task<object> PagarFacturaAsync(PagoFacturaRequest request, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                TurnoId = request.TurnoId,
                Metodo = request.MetodoPago,
                Monto = request.Monto
            };

            var response = await _http.PostAsync($"/api/factura/{request.FacturaId}/pago", Serializar(payload));
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<object> RegistrarAbonoAsync(AbonoCxCRequest request)
        {
            var response = await _http.PostAsync($"/api/cxc/{request.FacturaId}/abono", Serializar(request));
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task<CobroResponse> ProcesarCobroAsync(CobroRequest request, string token)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/facturas");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = Serializar(request);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return (await Deserializar<CobroResponse>(response))!;
        }

        // --- Clientes ---
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

            // HttpRequestMessage: el token va en esta petición únicamente
            var req = new HttpRequestMessage(HttpMethod.Post, "api/clientes");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(payload);

            var response = await _http.SendAsync(req);

            if (response.IsSuccessStatusCode)
                return true;

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG-REGISTRO] Status: {response.StatusCode} | Body: {json}");

            var error = JsonSerializer.Deserialize<ApiErrorWrapper>(json, _jsonOptions);
            throw new InvalidOperationException(error?.Error?.Mensaje ?? "No se pudo registrar el cliente.");

        }

        private async Task<T?> DeserializarRespuestaOCuerpo<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ApiWrapper<T>>(json, _jsonOptions);

            if (wrapper is not null && wrapper.Success && wrapper.Data is not null)
                return wrapper.Data;

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        // --- Facturas ---
        public async Task<IEnumerable<object>> ObtenerFacturasPorClienteAsync(int clienteId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/api/facturas/cliente/{clienteId}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.SendAsync(req);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<object>();

            var data = await Deserializar<IEnumerable<object>>(response);
            return data ?? Enumerable.Empty<object>();
        }

        // --- Vehículos ---
        public async Task<IEnumerable<object>> ObtenerVehiculosClienteAsync(int clienteId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"/api/vehiculos/cliente/{clienteId}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            var response = await _http.SendAsync(req);
            return await Deserializar<IEnumerable<object>>(response) ?? Enumerable.Empty<object>();
        }

        public async Task<object> ActualizarVehiculoAsync(int id, object vehiculo)
        {
            var req = new HttpRequestMessage(HttpMethod.Put, $"/api/vehiculos/{id}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(vehiculo);
            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }

        public async Task EliminarVehiculoAsync(int id)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"/api/vehiculos/{id}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            await _http.SendAsync(req);
        }

        public async Task<object> RegistrarVehiculoAsync(object vehiculo)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/vehiculos");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());
            req.Content = Serializar(vehiculo);

            var response = await _http.SendAsync(req);
            response.EnsureSuccessStatusCode();
            return await Deserializar<object>(response) ?? new { };
        }

        // --- Órdenes ---
        public async Task<IEnumerable<object>> ObtenerHistorialOrdenesAsync()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/api/ordenes/historial");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.SendAsync(req);
            return await Deserializar<IEnumerable<object>>(response) ?? Enumerable.Empty<object>();
        }
        public async Task<object> ObtenerFacturasPendientesAsync(string token, string? numeroFactura, string? placa)
        {
            var query = new List<string>();

            if (!string.IsNullOrEmpty(placa))
                query.Add($"placa={placa}");

            if (!string.IsNullOrEmpty(numeroFactura))
                query.Add($"numeroFactura={numeroFactura}");

            var url = "/api/factura/buscar";

            if (query.Count > 0)
                url += "?" + string.Join("&", query);


            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine(url);
            var response = await _http.GetAsync(url);
            return await Deserializar<object>(response) ?? new { };

        }


        // --- Catálogos ---
        public async Task<IEnumerable<ProductoDto>> ObtenerProductosAsync()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/api/productos");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.SendAsync(req);
            var data = await Deserializar<IEnumerable<ProductoDto>>(response);
            return data ?? Enumerable.Empty<ProductoDto>();
        }

        public async Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/api/servicios");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GenerarTokenInterno());

            var response = await _http.SendAsync(req);
            var data = await Deserializar<IEnumerable<ServicioDto>>(response);
            return data ?? Enumerable.Empty<ServicioDto>();
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

        // --- Token interno para llamadas Core ↔ Integración ---
        private string GenerarTokenInterno()
        {
            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes("TallerFrenosClaveSecreta2026!MuyLargaParaHMAC256"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TallerCore",
                audience: "TallerIntegracion",
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "0"),
                    new Claim(JwtRegisteredClaimNames.Email, "integracion@sistema.interno"),
                    new Claim("Rol", "Admin")
                },
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    } }

    internal record ApiErrorDto(string Codigo, string Mensaje);
    internal record ApiErrorWrapper(bool Success, object? Data, ApiErrorDto? Error);
    internal record ApiWrapper<T>(bool Success, T? Data, object? Error);
    internal record TokenDto(string Token);
    internal record AbrirTurnoCoreResponse(
        int TurnoId,
        string Estado,
        DateTime FechaApertura);
    internal record CerrarTurnoCoreResponse(
        int TurnoId,
        string Estado,
        DateTime FechaApertura,
        DateTime FechaCierre,
        decimal MontoInicial,
        decimal EfectivoContado,
        string? Observaciones);
