using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace FrenosWeb.Services
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        // ✅ Guardamos el estado actual en memoria para evitar la condición de carrera
        private AuthenticationState _currentState;

        public TestAuthStateProvider(IJSRuntime js)
        {
            _js = js;
            _currentState = _anonymous;
        }

        // ✅ Este método es llamado por [Authorize] y AuthorizeView
        // Ahora lee el token del localStorage Y actualiza el estado en memoria
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrWhiteSpace(token))
                {
                    _currentState = _anonymous;
                    return _anonymous;
                }

                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);
                _currentState = new AuthenticationState(user);
                return _currentState;
            }
            catch
            {
                _currentState = _anonymous;
                return _anonymous;
            }
        }

        public void NotifyUserAuthentication(string email)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email)
            }, "jwt");
            var user = new ClaimsPrincipal(identity);
            _currentState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
        }

        public void NotifyUserLogout()
        {
            _currentState = _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            try
            {
                var payload = jwt.Split('.')[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }
                var jsonBytes = Convert.FromBase64String(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                if (keyValuePairs != null)
                {
                    foreach (var kvp in keyValuePairs)
                    {
                        switch (kvp.Key)
                        {
                            case "role":
                                claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString() ?? ""));
                                break;
                            case "unique_name":
                            case "email":
                                claims.Add(new Claim(ClaimTypes.Name, kvp.Value.ToString() ?? ""));
                                claims.Add(new Claim(ClaimTypes.Email, kvp.Value.ToString() ?? ""));
                                break;
                            case "nameid":
                                claims.Add(new Claim(ClaimTypes.NameIdentifier, kvp.Value.ToString() ?? ""));
                                break;
                            default:
                                claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
                                break;
                        }
                    }
                }
            }
            catch { }
            return claims;
        }
    }
}