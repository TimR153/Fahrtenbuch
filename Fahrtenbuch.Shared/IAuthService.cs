namespace Fahrtenbuch.Shared
{
    public interface IAuthService
    {
        Task LoginAsync(string returnUrl = "/");
        Task LogoutAsync();
    }
}
