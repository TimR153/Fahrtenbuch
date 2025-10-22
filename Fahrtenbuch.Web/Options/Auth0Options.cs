namespace Fahrtenbuch.Web.Options
{
    public class Auth0Options
    {
        public static string RootElement { get; set; } = "Auth0";
        public string Domain { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
    }
}
