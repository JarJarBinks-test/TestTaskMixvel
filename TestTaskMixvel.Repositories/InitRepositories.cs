using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTaskMixvel.Repositories.Interfaces.Cache;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Repositories.Cache;
using TestTaskMixvel.Repositories.Repositories.Search;

namespace TestTaskMixvel.Repositories
{
    /// <summary>
    /// Static class for extention methods for repositories.
    /// </summary>
    public static class InitRepositories
    {
        /// <summary>
        /// Add repositories and contexts to services collections.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("ProviderOneClient", (serviceProvider, client) => {
                var providerOneBaseUrl = configuration["Providers:ProviderOneBaseUrl"] ?? throw new Exception("Providers:ProviderOneBaseUrl not defined in config.");
                client.BaseAddress = new Uri(providerOneBaseUrl);
            });
            services.AddHttpClient("ProviderTwoClient", (serviceProvider, client) => {
                var providerTwoBaseUrl = configuration["Providers:ProviderTwoBaseUrl"] ?? throw new Exception("Providers:ProviderTwoBaseUrl not defined in config.");
                client.BaseAddress = new Uri(providerTwoBaseUrl);
            });

            services.AddStackExchangeRedisCache(options => {
                var redisCacheConnectionString = configuration["RedisCacheConnectionString"] ?? throw new Exception("RedisCacheConnectionString not defined in config.");                
                options.Configuration = redisCacheConnectionString;
            });

            services.AddTransient<IProviderOneRepository, ProviderOneRepository>();
            services.AddTransient<IProviderTwoRepository, ProviderTwoRepository>();
            services.AddTransient<IRedisCacheRepository, RedisCacheRepository>();
        }
    }
}
