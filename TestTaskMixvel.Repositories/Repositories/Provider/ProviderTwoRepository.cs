using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Models.Provider.ProviderTwo;

namespace TestTaskMixvel.Repositories.Repositories.Search
{
    /// <summary>
    /// Class for access to provider two.
    /// </summary>
    /// <seealso cref="TestTaskMixvel.Repositories.Interfaces.Provider.IProviderTwoRepository" />
    public class ProviderTwoRepository : IProviderTwoRepository
    {
        readonly ILogger<ProviderTwoRepository> _logger;
        readonly IHttpClientFactory _httpFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderTwoRepository"/> class.
        /// </summary>
        /// <param name="httpFactory">The HTTP factory.</param>
        /// <param name="logger">The logger.</param>
        public ProviderTwoRepository(IHttpClientFactory httpFactory, ILogger<ProviderTwoRepository> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProviderTwoSearchResponse> SearchAsync(ProviderTwoSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            try
            {
                var client = _httpFactory.CreateClient("ProviderTwoClient");
                var response = await client.PostAsJsonAsync("search", request, cancellationToken);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>(cancellationToken);
                if (result == null)
                {
                    throw new Exception("Result is null or not parsed correctly.");
                }
                return result;
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
            _logger.LogInformation($"{nameof(IsAvailableAsync)}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {DateTime.UtcNow}.");
            try
            {
                var client = _httpFactory.CreateClient("ProviderTwoClient");
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
