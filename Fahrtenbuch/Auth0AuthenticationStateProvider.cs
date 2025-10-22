using Auth0.OidcClient;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Fahrtenbuch
{
    public class Auth0AuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(_user));

        public void NotifyUserAuthentication(ClaimsPrincipal user)
        {
            _user = user ?? new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
