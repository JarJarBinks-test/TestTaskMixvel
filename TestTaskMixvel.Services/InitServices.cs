using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Services.Search;
using TestTaskMixvel.Repositories;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Services.Adapters.Search;

namespace TestTaskMixvel.Services
{
    /// <summary>
    /// Static class for extention methods for service.
    /// </summary>
    public static class InitServices
    {
        /// <summary>
        /// Add services to services collections.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IProviderAdapter<IProviderOneRepository>, ProviderOneRepositoryAdapter>();
            services.AddTransient<IProviderAdapter<IProviderTwoRepository>, ProviderTwoRepositoryAdapter>();

        }
    }
}
