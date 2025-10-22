using Auth0.AspNetCore.Authentication;
using Fahrtenbuch.Shared;
using Fahrtenbuch.Shared.Services;
using Fahrtenbuch.Web.AuthenticationStateSyncer;
using Fahrtenbuch.Web.Components;
using Fahrtenbuch.Web.Options;
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

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        ConfigureMiddleware(app);

        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddHttpContextAccessor();
        services.AddSingleton<IFormFactor, FormFactor>();
        services.AddScoped<IAuthService, BlazorAuthService>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        services.AddScoped<TokenHandler>();
        services.AddMudServices();
        services.AddCascadingAuthenticationState();

        RegisterAuth0(services, configuration);

        services.Configure<ApiOptions>(configuration.GetSection(ApiOptions.RootElement));
        var apiOptions = configuration.GetSection(ApiOptions.RootElement).Get<ApiOptions>() ?? new ApiOptions();

        if (!string.IsNullOrEmpty(apiOptions.BaseUrl))
        {
            Fahrtenbuch.Web.Extensions.ServiceCollectionExtensions.AddHttpClient<IUserInfoClient, UserInfoClient>(services, apiOptions.BaseUrl);
            Fahrtenbuch.Web.Extensions.ServiceCollectionExtensions.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(services, apiOptions.BaseUrl);
        }
    }

    private static void RegisterAuth0(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Auth0Options>(configuration.GetSection(Auth0Options.RootElement));
        var auth0Options = configuration.GetSection(Auth0Options.RootElement).Get<Auth0Options>() ?? new Auth0Options();

        services.AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = auth0Options.Domain;
            options.ClientId = auth0Options.ClientId;
            options.ClientSecret = auth0Options.ClientSecret;
            options.Scope = auth0Options.Scope;
        })
        .WithAccessToken(options =>
        {
            options.Audience = auth0Options.Audience;
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet(Constants.LoginUri, async (HttpContext httpContext, string returnUrl = "/") =>
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        });

        app.MapGet(Constants.LogoutUri, async (HttpContext httpContext) =>
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();

            await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });

        app.MapGet(Constants.InternalDataUri, () =>
        {
            var data = Enumerable.Range(1, 5).Select(_ => Random.Shared.Next(1, 100)).ToArray();
            return data;
        }).RequireAuthorization();

        app.MapGet(Constants.ExternalDataUri, async (HttpClient httpClient) =>
        {
            return await httpClient.GetFromJsonAsync<int[]>("data");
        }).RequireAuthorization();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(typeof(Fahrtenbuch.Shared._Imports).Assembly);
    }
}