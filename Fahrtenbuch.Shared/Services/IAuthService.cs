namespace Fahrtenbuch.Shared.Services
{
    public interface IAuthService
    {
        Task LoginAsync(string returnUrl = "/");
        Task LogoutAsync();
    }
}
