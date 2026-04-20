using FrenosWeb.Models;
using System.Net.Http.Json;

namespace FrenosWeb.Services
{
    public class AuthService(HttpClient http)
    {
        public bool IsLoggedIn { get; private set; }
        public UserSession? CurrentUser { get; private set; }

        public async Task<bool> Login(LoginModel model)
        {
            try
            {
                // 🛡️ 1. Intento Real: Hablar con la API
                var response = await http.PostAsJsonAsync("api/auth/login", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserSession>();
                    if (result != null)
                    {
                        EstablecerSesion(result);
                        return true;
                    }
                }

                // Si la API responde pero dice que las credenciales están mal
                return false;
            }
            catch (Exception ex)
            {
                // 🚨 2. Fallover: Si la API no responde (servidor caído), usamos el simulador
                Console.WriteLine($"[Cyber-Logs] API de Auth no disponible: {ex.Message}");

                await Task.Delay(1000); // Simulamos latencia

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

        private void EstablecerSesion(UserSession session)
        {
            IsLoggedIn = true;
            CurrentUser = session;
            // Aquí podrías guardar el token en el LocalStorage más adelante
        }

        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUser = null;
        }
    }
}