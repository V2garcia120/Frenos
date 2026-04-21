using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FrenosWeb.Services
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState _currentState;

        public TestAuthStateProvider()
        {
            _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(_currentState);

        public void NotifyAuthenticationStateChanged(string? email)
        {
            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(email))
            {
                identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            _currentState = new AuthenticationState(new ClaimsPrincipal(identity));
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
        }
    }
}