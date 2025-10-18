using Auth0.OidcClient;
using Fahrtenbuch.Shared.Services;
using System.Security.Claims;

namespace Fahrtenbuch.Services
{
    public class MauiAuthService : IAuthService
    {
        private const string AccessTokenKey = "access_token";
        private const string IdTokenKey = "id_token";

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

            await SecureStorage.SetAsync(AccessTokenKey, loginResult.AccessToken);
            await SecureStorage.SetAsync(IdTokenKey, loginResult.IdentityToken);

            var identity = new ClaimsIdentity(loginResult.User.Claims, Constants.Auth0Claim);
            var user = new ClaimsPrincipal(identity);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _authStateProvider.NotifyUserAuthentication(user);
            });

        }

        public async Task LogoutAsync()
        {
            await _auth0Client.LogoutAsync();

            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(IdTokenKey);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _authStateProvider.NotifyUserLogout();
            });

        }
    }
}
