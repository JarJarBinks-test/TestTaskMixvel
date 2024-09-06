using TestTaskMixvel.Repositories.Models.Provider.ProviderTwo;

namespace TestTaskMixvel.Repositories.Interfaces.Provider
{
    /// <summary>
    /// Interface for provider two repository.
    /// </summary>
    public interface IProviderTwoRepository
    {
        /// <summary>
        /// Searches the routes.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Search response.</returns>
        Task<ProviderTwoSearchResponse> SearchAsync(ProviderTwoSearchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether is available.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if available.</returns>
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
