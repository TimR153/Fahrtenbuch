using Fahrtenbuch.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Fahrtenbuch.Web.Services
{
    public class BlazorAuthService : IAuthService
    {
        private readonly NavigationManager _navigationManager;

        public BlazorAuthService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public Task LoginAsync(string returnUrl = "/")
        {
            _navigationManager.NavigateTo(Constants.LoginUri, forceLoad: true);
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            _navigationManager.NavigateTo(Constants.LogoutPath, forceLoad: true);
            return Task.CompletedTask;
        }
    }
}
