using Android.App;
using Microsoft.Identity.Client;
using Android.Content;

namespace Fahrtenbuch.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] {
            Intent.CategoryBrowsable,
            Intent.CategoryDefault
        },
        DataScheme = "msal" + "4205584b-9e9b-4d5c-b810-19521aa6e994",
        DataHost = "auth")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
