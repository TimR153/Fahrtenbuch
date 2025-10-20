namespace Fahrtenbuch.Web.Api.Models
{
    public class UserInfo
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        public string? Locale { get; set; }
        public string? Picture { get; set; }
        public List<string>? Roles { get; set; }
    }
}
