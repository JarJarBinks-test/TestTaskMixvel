using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Controllers.Search
{
    /// <summary>
    /// Search routes controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]/[action]")]
    public class SearchRouteController : ControllerBase
    {
        readonly ILogger<SearchRouteController> _logger;
        readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRouteController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="logger">The logger.</param>
        public SearchRouteController(ISearchService searchService, ILogger<SearchRouteController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Searches the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Search response.</returns>
        [HttpPost]
        public Task<SearchResponse> Search([Required] SearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Search)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. Body: {JsonSerializer.Serialize(request)}");

            if (!ModelState.IsValid)
            {
                throw new Exception($"Request invalid.{string.Join(", ", ModelState.Keys.ToArray())}.");
            }

            return _searchService.SearchAsync(request, cancellationToken);
        }

        /// <summary>
        /// Determines whether the service is available.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if available.</returns>
        [HttpGet]
        public Task<bool> IsAvailable(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IsAvailable)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}.");

            return _searchService.IsAvailableAsync(cancellationToken);
        }

    }
}
