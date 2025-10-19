using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Fahrtenbuch.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHttpClient<TClient, TImplementation>(
            this IServiceCollection services,
            string baseUrl)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TImplementation>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                return (TClient)Activator.CreateInstance(typeof(TImplementation), new object[] { baseUrl, httpClient });
            });
        }

        public static void AddHttpClient<TClient, TImplementation>(
            this IServiceCollection services,
            string baseUrl,
            string accessToken)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TImplementation>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken);
                }
            });

            services.AddScoped(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                return (TClient)Activator.CreateInstance(
                    typeof(TImplementation), new object[] { baseUrl, accessToken, httpClient });
            });
        }
    }
}
