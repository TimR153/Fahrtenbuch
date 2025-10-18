using Auth0.OidcClient;
using Fahrtenbuch.Shared.Services;
using System.Security.Claims;

namespace Fahrtenbuch
{
    public class MauiAuthService : IAuthService
    {
        private readonly Auth0Client _auth0Client;
        private readonly Auth0AuthenticationStateProvider _authStateProvider;

        public MauiAuthService(Auth0AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "tim-rentrop.eu.auth0.com",
                ClientId = "aVw8W49w7ix5ikIrbJV83pYPaL2zVMsF",
                Scope = "openid profile email",
                RedirectUri = "fahrtenbuch://callback/",
                PostLogoutRedirectUri = "fahrtenbuch://callback/"
            });
        }

        public async Task LoginAsync(string returnUrl = "/")
        {
            var loginResult = await _auth0Client.LoginAsync();
            if (loginResult.IsError)
            {
                throw new Exception(loginResult.Error);
            }

            await SecureStorage.SetAsync("access_token", loginResult.AccessToken);
            await SecureStorage.SetAsync("id_token", loginResult.IdentityToken);

            var identity = new ClaimsIdentity(loginResult.User.Claims, "Auth0");
            var user = new ClaimsPrincipal(identity);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _authStateProvider.NotifyUserAuthentication(user);
            });

        }

        public async Task LogoutAsync()
        {
            await _auth0Client.LogoutAsync();

            SecureStorage.Remove("access_token");
            SecureStorage.Remove("id_token");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _authStateProvider.NotifyUserLogout();
            });

        }
    }
}
