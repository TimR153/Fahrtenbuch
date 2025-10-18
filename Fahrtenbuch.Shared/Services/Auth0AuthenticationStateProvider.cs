using Auth0.OidcClient;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Fahrtenbuch.Shared.Services
{
    public class Auth0AuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuth0Client _auth0Client;
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());
        public Auth0AuthenticationStateProvider(IAuth0Client auth0Client) => _auth0Client = auth0Client;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(_user));

        public async Task LogInAsync()
        {
            var loginResult = await _auth0Client.LoginAsync();
            if (!loginResult.IsError)
                _user = loginResult.User;
            else
                _user = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogOutAsync()
        {
            await _auth0Client.LogoutAsync();
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
