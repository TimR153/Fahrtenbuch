using Auth0.AspNetCore.Authentication;
using Fahrtenbuch.Shared;
using Fahrtenbuch.Shared.Services;
using Fahrtenbuch.Web.AuthenticationStateSyncer;
using Fahrtenbuch.Web.Components;
using Fahrtenbuch.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddScoped<IAuthService, BlazorAuthService>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        builder.Services.AddScoped<TokenHandler>();
        builder.Services.AddMudServices();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = builder.Configuration["Auth0:Domain"];
            options.ClientId = builder.Configuration["Auth0:ClientId"];
            options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
            options.Scope = "openid profile email";
        })
            .WithAccessToken(options =>
            {
                options.Audience = builder.Configuration["Auth0:Audience"];
            }); ;
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapGet(Fahrtenbuch.Web.Constants.LoginPath, async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });

        app.MapGet(Fahrtenbuch.Web.Constants.LogoutPath, async (httpContext) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                    .WithRedirectUri("/")
                    .Build();

            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });

        app.MapGet("/api/internalData", () =>
        {
            var data = Enumerable.Range(1, 5).Select(index =>
                Random.Shared.Next(1, 100))
                .ToArray();

            return data;
        })
        .RequireAuthorization();

        app.MapGet("/api/externalData", async (HttpClient httpClient) =>
        {
            return await httpClient.GetFromJsonAsync<int[]>("data");
        })
        .RequireAuthorization();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Fahrtenbuch.Shared._Imports).Assembly);

        app.Run();
    }

    static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");

        if (baseUrl != null)
        {
            Fahrtenbuch.Web.Extensions.ServiceCollectionExtensions.AddHttpClient<IUserInfoClient, UserInfoClient>(services, baseUrl);
            Fahrtenbuch.Web.Extensions.ServiceCollectionExtensions.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(services, baseUrl);
        }
    }
}