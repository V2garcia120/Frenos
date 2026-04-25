using FrenosWeb.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop; 
using System.Net.Http.Headers;

namespace FrenosWeb.Services
{
    public class AuthService(HttpClient http, AuthenticationStateProvider authProvider, IJSRuntime js)
    {
        public bool IsLoggedIn { get; private set; }
        public UserSession? CurrentUser { get; private set; }

        private async Task EstablecerSesion(UserSession session)
        {
            IsLoggedIn = true;
            CurrentUser = session;

            // 1. Guardamos el Token en el LocalStorage para que no se borre al refrescar
            await js.InvokeVoidAsync("localStorage.setItem", "authToken", session.Token);

            // 2. Configuramos el HttpClient para que TODAS las futuras llamadas lleven el token
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);

            // 3. Avisamos al sistema de Blazor que el usuario cambió
            if (authProvider is TestAuthStateProvider testProvider)
            {
                testProvider.NotifyUserAuthentication(session.Email);
            }
        }

        public async Task Logout()
        {
            IsLoggedIn = false;
            CurrentUser = null;

            // Limpiamos el rastro
            await js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            http.DefaultRequestHeaders.Authorization = null;

            if (authProvider is TestAuthStateProvider testProvider)
            {
                testProvider.NotifyUserLogout();
            }
        }

        public async Task<bool> Login(LoginModel model)
        {
            try
            {
                // Llamada al endpoint de Integración 
                var response = await http.PostAsJsonAsync("int/auth/login-cliente", new
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserSession>>();

                    if (result != null && result.Success && result.Data != null)
                    {
                        await EstablecerSesion(result.Data);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error de Red: {ex.Message}");

                // Modo Emergencia
                if (model.Email == "admin@frenos.com" && model.Password == "123456")
                {
                    var fakeToken = GenerarMockJwt(model.Email);
                    await EstablecerSesion(new UserSession
                    {
                        Email = model.Email,
                        Rol = "Cliente",
                        Token = fakeToken
                    });
                    return true;
                }
                return false;
            }
        }

        private string GenerarMockJwt(string email)
        {
            var header = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
            var payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{{\"unique_name\":\"{email}\",\"role\":\"Cliente\",\"exp\":1735689600}}"));
            var signature = "fake_signature_part";

            return $"{header}.{payload}.{signature}";
        }

        public bool Inicializado { get; private set; } = false;

        public async Task InicializarSesionDesdeStorage()
        {
            var token = await js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                IsLoggedIn = true;
            }
            Inicializado = true;
        }

        public async Task<(bool Exito, string? Error)> Registrar(ClienteModel modelo)
        {
            try
            {
                var response = await http.PostAsJsonAsync("int/auth/registrar-cliente", modelo);

                if (response.IsSuccessStatusCode)
                    return (true, null);

                var json = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<object>>(
                    json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return (false, result?.Error?.Mensaje ?? "No se pudo crear la cuenta.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error de conexión: {ex.Message}");
                return (false, "Error de conexión con el servidor.");
            }
        }
    }
}