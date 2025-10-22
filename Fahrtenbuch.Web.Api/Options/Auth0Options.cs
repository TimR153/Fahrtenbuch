namespace Fahrtenbuch.Web.Api.Options
{
    public class Auth0Options
    {
        public static string RootElement { get; set; } = "Auth0";
        public string Domain { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
