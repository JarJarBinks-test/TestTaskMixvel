using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Services.Interfaces.Search
{
    /// <summary>
    /// Interface of search routes service.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Searches the routes.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Search response.</returns>
        Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether is available.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if available.</returns>
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}