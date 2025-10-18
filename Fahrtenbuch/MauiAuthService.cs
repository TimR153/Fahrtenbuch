using Auth0.OidcClient;
using Fahrtenbuch.Shared;

namespace Fahrtenbuch
{
    public class MauiAuthService : IAuthService
    {
        private readonly Auth0Client _auth0Client;

        public MauiAuthService()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "tim-rentrop.eu.auth0.com",
                ClientId = "aVw8W49w7ix5ikIrbJV83pYPaL2zVMsF",
                Scope = "openid profile",
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
        }

        public async Task LogoutAsync()
        {
            await _auth0Client.LogoutAsync();
        }
    }
}
