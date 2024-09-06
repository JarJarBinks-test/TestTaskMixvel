using Microsoft.Extensions.Logging;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Models.Provider.ProviderOne;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Services.Adapters.Search
{
    /// <summary>
    /// Adapter for provider one. 
    /// </summary>
    /// <seealso cref="IProviderAdapter&lt;IProviderOneRepository&gt;" />
    public class ProviderOneRepositoryAdapter : IProviderAdapter<IProviderOneRepository>
    {
        readonly ILogger<ProviderOneRepositoryAdapter> _logger;
        readonly IProviderOneRepository _providerOneRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderOneRepositoryAdapter"/> class.
        /// </summary>
        /// <param name="providerOneRepository">The provider one repository.</param>
        /// <param name="logger">The logger.</param>
        public ProviderOneRepositoryAdapter(IProviderOneRepository providerOneRepository, ILogger<ProviderOneRepositoryAdapter> logger)
        {
            _providerOneRepository = providerOneRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            var providerSearchRequest = SearchRequestToProviderOneSearchResuqest(request);
            var result = await _providerOneRepository.SearchAsync(providerSearchRequest, cancellationToken);
            return ProviderSearchResponseToSearchResponse(result, request);
        }

        /// <inheritdoc />
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IsAvailableAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}.");

            return _providerOneRepository.IsAvailableAsync(cancellationToken);
        }

        /// <summary>
        /// Searches the request to provider one search resuqest.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Provider one search request.</returns>
        ProviderOneSearchRequest SearchRequestToProviderOneSearchResuqest(SearchRequest request)
        {
            return new ProviderOneSearchRequest()
            {
                From = request.Origin,
                To = request.Destination,
                DateFrom = request.OriginDateTime,
                MaxPrice = request.Filters?.MaxPrice,
                DateTo = request.Filters?.DestinationDateTime,
            };
        }

        /// <summary>
        /// Providers the search response to search response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        SearchResponse ProviderSearchResponseToSearchResponse(ProviderOneSearchResponse response, SearchRequest request)
        {
            var routes = response.Routes
                .Where(route => request.Filters?.MinTimeLimit == null || route.TimeLimit >= request.Filters.MinTimeLimit.Value) // Additional filtering.
                .Select(route => new Route()
                {
                    Destination = route.To,
                    DestinationDateTime = route.DateTo,
                    Origin = route.From,
                    OriginDateTime = route.DateFrom,
                    Price = route.Price,
                    TimeLimit = route.TimeLimit,
                }).ToArray();
            return new SearchResponse()
            {
                Routes = routes,
                MinPrice = routes.Min(route => route.Price),
                MaxPrice = routes.Max(route => route.Price),
                MinMinutesRoute = Convert.ToInt32(routes.Min(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
                MaxMinutesRoute = Convert.ToInt32(routes.Max(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
            };
        }
    }
}
