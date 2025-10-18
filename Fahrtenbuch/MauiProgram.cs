using Auth0.OidcClient;
using Fahrtenbuch.Services;
using Fahrtenbuch.Shared;
using Fahrtenbuch.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace Fahrtenbuch
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMudServices();
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
            builder.Services.AddSingleton<IAuthService, MauiAuthService>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
