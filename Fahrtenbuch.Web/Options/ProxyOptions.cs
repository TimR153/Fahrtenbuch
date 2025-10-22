namespace Fahrtenbuch.Web.Options
{
    public class ProxyOptions
    {
        public static string RootElement { get; set; } = "Proxy";
        public string KnownProxies { get; set; } = string.Empty;
    }
}
