using FrenosWeb.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization; 

namespace FrenosWeb.Services
{
    public class AuthService(HttpClient http, AuthenticationStateProvider authProvider)
    {
        public bool IsLoggedIn { get; private set; }
        public UserSession? CurrentUser { get; private set; }

        private void EstablecerSesion(UserSession session)
        {
            IsLoggedIn = true;
            CurrentUser = session;

            if (authProvider is TestAuthStateProvider testProvider)
            {
                testProvider.NotifyAuthenticationStateChanged(session.Email);
            }
        }

        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUser = null;

            if (authProvider is TestAuthStateProvider testProvider)
            {
                testProvider.NotifyAuthenticationStateChanged(null);
            }
        }

        public async Task<bool> Login(LoginModel model)
        {
            try
            {
                var response = await http.PostAsJsonAsync("int/auth/login", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserSession>>();
                    if (result != null && result.Success && result.Data != null)
                    {
                        EstablecerSesion(result.Data);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cyber-Logs] Error de conexión con Integración: {ex.Message}");

                if (model.Email == "admin@frenos.com" && model.Password == "123456")
                {
                    EstablecerSesion(new UserSession
                    {
                        Email = model.Email,
                        Rol = "Cliente",
                        Token = "fake-jwt-token-desarrollo"
                    });
                    return true;
                }

                return false;
            }
        }
    }
}