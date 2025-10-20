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
            })
            .AddHttpMessageHandler<TokenHandler>();

            services.AddScoped<TClient>(provider =>
            {
                var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(TImplementation).Name);

                return (TClient)Activator.CreateInstance(typeof(TImplementation), new object[] { baseUrl, httpClient });
            });
        }
    }
}
