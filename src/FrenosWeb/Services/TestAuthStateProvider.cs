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

        public TestAuthStateProvider(IJSRuntime js) => _js = js;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrWhiteSpace(token))
                    return _anonymous;

                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return _anonymous;
            }
        }

        public void NotifyUserAuthentication(string email)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return new List<Claim>();

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}