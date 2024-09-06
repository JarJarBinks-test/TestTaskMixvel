using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Models.Provider.ProviderOne;

namespace TestTaskMixvel.Repositories.Repositories.Search
{
    /// <summary>
    /// Class for access to provider one.
    /// </summary>
    /// <seealso cref="IProviderOneRepository" />
    public class ProviderOneRepository : IProviderOneRepository
    {
        readonly ILogger<ProviderOneRepository> _logger;
        readonly IHttpClientFactory _httpFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderOneRepository"/> class.
        /// </summary>
        /// <param name="httpFactory">The HTTP factory.</param>
        /// <param name="logger">The logger.</param>
        public ProviderOneRepository(IHttpClientFactory httpFactory, ILogger<ProviderOneRepository> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProviderOneSearchResponse> SearchAsync(ProviderOneSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            try
            {
                var client = _httpFactory.CreateClient("ProviderOneClient");
                var response = await client.PostAsJsonAsync("search", request, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Message: {ex.Message}, Stack: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IsAvailableAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}.");
            try
            {
                var client = _httpFactory.CreateClient("ProviderOneClient");
                var response = await client.GetAsync("ping", cancellationToken);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(IsAvailableAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Message: {ex.Message}, Stack: {ex.StackTrace}");
                throw;
            }
        }
    }
}
