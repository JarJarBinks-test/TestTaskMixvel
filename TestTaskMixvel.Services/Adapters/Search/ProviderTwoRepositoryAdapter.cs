using Microsoft.Extensions.Logging;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Models.Provider.ProviderTwo;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Services.Adapters.Search
{
    /// <summary>
    /// Adapter for provider two. 
    /// </summary>
    /// <seealso cref="IProviderAdapter&lt;IProviderTwoRepository&gt;" />
    public class ProviderTwoRepositoryAdapter : IProviderAdapter<IProviderTwoRepository>
    {
        readonly ILogger<ProviderTwoRepositoryAdapter> _logger;
        readonly IProviderTwoRepository _providerTwoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderTwoRepositoryAdapter"/> class.
        /// </summary>
        /// <param name="providerTwoRepository">The provider two repository.</param>
        /// <param name="logger">The logger.</param>
        public ProviderTwoRepositoryAdapter(IProviderTwoRepository providerTwoRepository, ILogger<ProviderTwoRepositoryAdapter> logger)
        {
            _providerTwoRepository = providerTwoRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            var providerSearchRequest = SearchRequestToProviderTwoSearchResuqest(request);
            var result = await _providerTwoRepository.SearchAsync(providerSearchRequest, cancellationToken);
            return ProviderSearchResponseToSearchResponse(result, request);
        }

        /// <inheritdoc />
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IsAvailableAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}.");

            return _providerTwoRepository.IsAvailableAsync(cancellationToken);
        }

        /// <summary>
        /// Searches the request to provider Two search resuqest.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Provider Two search request.</returns>
        ProviderTwoSearchRequest SearchRequestToProviderTwoSearchResuqest(SearchRequest request)
        {
            return new ProviderTwoSearchRequest()
            {
                Departure = request.Origin,
                DepartureDate = request.OriginDateTime,
                Arrival = request.Destination,
                MinTimeLimit = request.Filters?.MinTimeLimit,
            };
        }

        /// <summary>
        /// Providers the search response to search response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        SearchResponse ProviderSearchResponseToSearchResponse(ProviderTwoSearchResponse response, SearchRequest request)
        {
            var routes = response.Routes
                .Where(route => (request.Filters?.DestinationDateTime == null || route.Arrival.Date <= request.Filters.DestinationDateTime.Value) &&
                    (request.Filters?.MaxPrice == null || route.Price <= request.Filters.MaxPrice.Value)) // Additional filtering.
                .Select(route => new Route()
                {
                    Origin = route.Departure.Point,
                    OriginDateTime = route.Departure.Date,
                    Destination = route.Arrival.Point,
                    DestinationDateTime = route.Arrival.Date,
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
