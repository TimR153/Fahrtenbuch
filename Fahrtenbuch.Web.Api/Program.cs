using Fahrtenbuch.Infrastructure;
using Fahrtenbuch.Web.Api;
using Fahrtenbuch.Web.Api.Authentification;
using Fahrtenbuch.Web.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        ConfigureAuth0(builder.Services, builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "v1";
            config.Title = Constants.AppName;
            config.Version = "v1";
        });

        builder.Services.AddDbContext<FahrtenbuchDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(Constants.DefaultConnectionStringKey)));

        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    }

    private static void ConfigureAuth0(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Auth0Options>(configuration.GetSection(Auth0Options.RootElement));
        var auth0Options = configuration.GetSection(Auth0Options.RootElement).Get<Auth0Options>() ?? new Auth0Options();

        var domain = $"https://{auth0Options.Domain}/";
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = auth0Options.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}