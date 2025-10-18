using Fahrtenbuch.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Fahrtenbuch.Web
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
            _navigationManager.NavigateTo("/Account/Login", forceLoad: true);
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            _navigationManager.NavigateTo("/Account/Logout", forceLoad: true);
            return Task.CompletedTask;
        }
    }
}
