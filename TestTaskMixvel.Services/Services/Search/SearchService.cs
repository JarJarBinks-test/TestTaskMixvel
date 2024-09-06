using Microsoft.Extensions.Logging;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Cache;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Services.Services.Search
{
    /// <summary>
    /// Service for search routes.
    /// </summary>
    /// <seealso cref="TestTaskMixvel.Services.Interfaces.Search.ISearchService" />
    public class SearchService: ISearchService
    {
        readonly ILogger<SearchService> _logger;
        readonly IProviderAdapter<IProviderOneRepository> _providerOneAdapter;
        readonly IProviderAdapter<IProviderTwoRepository> _providerTwoAdapter;
        readonly IRedisCacheRepository _redisCacheRepository;

        List<ISearchService> ProvidersAdapters
        {
            get
            {
                return new List<ISearchService>()
                {
                    _providerOneAdapter,
                    _providerTwoAdapter,
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchService"/> class.
        /// </summary>
        /// <param name="providerOneAdapter">The provider one repository adapter.</param>
        /// <param name="providerTwoAdapter">The provider two repository adapter.</param>
        /// <param name="redisCacheRepository">The redis cache repository.</param>
        /// <param name="logger">The logger.</param>
        public SearchService(IProviderAdapter<IProviderOneRepository> providerOneAdapter, IProviderAdapter<IProviderTwoRepository> providerTwoAdapter,
            IRedisCacheRepository redisCacheRepository, ILogger<SearchService> logger)
        {
            _providerOneAdapter = providerOneAdapter;
            _providerTwoAdapter = providerTwoAdapter;
            _redisCacheRepository = redisCacheRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(SearchAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            var cachedData = await _redisCacheRepository.Get<SearchResponse>(request.UniqueKey, cancellationToken);
            if (request.Filters?.OnlyCached == true || cachedData != null)
            {
                return cachedData ?? new SearchResponse();
            }

            var requestsTasks = ProvidersAdapters
                .Select(provider => provider.SearchAsync(request, cancellationToken).ContinueWith(task => task.IsFaulted ? null : task.Result))
                .ToArray();
            await Task.WhenAll(requestsTasks);

            var responses = requestsTasks
                .Select(task => task.Result)
                .Where(result => result != null)
                .ToList();
            var routes = responses
                .SelectMany(response => response.Routes)
                .GroupBy(route =>
                new
                {
                    route.TimeLimit,
                    route.Price,
                    route.Destination,
                    route.DestinationDateTime,
                    route.Origin,
                    route.OriginDateTime
                })
                .Select(groups => groups.First())
                .ToArray();

            if (routes.Length == 0)
            {
                return new SearchResponse();
            }
            else
            {
                var response = new SearchResponse()
                {
                    Routes = routes,
                    MinPrice = routes.Min(route => route.Price),
                    MaxPrice = routes.Max(route => route.Price),
                    MinMinutesRoute = Convert.ToInt32(routes.Min(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
                    MaxMinutesRoute = Convert.ToInt32(routes.Max(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
                };
                var timeLimit = routes.Min(route => route.TimeLimit);
                await _redisCacheRepository.Set(request.UniqueKey, response, timeLimit, cancellationToken);
                return response;
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IsAvailableAsync)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}.");

            var requestsTasks = ProvidersAdapters.Select(provider => provider.IsAvailableAsync(cancellationToken).ContinueWith(task => task.IsFaulted ? false : task.Result)).ToArray();
            await Task.WhenAll(requestsTasks);
            return requestsTasks.Any(task => task.Result);
        }
    }
}