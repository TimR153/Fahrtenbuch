namespace Fahrtenbuch
{
    public static class EntraConfig
    {
        public static string Authority = "https://timrentroptonline.ciamlogin.com/";
        public static string ClientId = "4205584b-9e9b-4d5c-b810-19521aa6e994";
        public static string[] Scopes = { "oppenid", "offline_access" };
        public static string IOSKeychainSecurityGroup = "com.microsoft.adalcache"; 
        public static object? ParentWindow { get; set; }
    }
}
