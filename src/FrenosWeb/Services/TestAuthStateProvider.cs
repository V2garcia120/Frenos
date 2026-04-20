using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FrenosWeb.Services
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Por defecto, devolvemos un usuario anónimo (no logueado)
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }
    }
}