using TestTaskMixvel.Repositories.Models.Provider.ProviderOne;

namespace TestTaskMixvel.Repositories.Interfaces.Provider
{
    /// <summary>
    /// Interface for provider one repository.
    /// </summary>
    public interface IProviderOneRepository
    {
        /// <summary>
        /// Searches the routes.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Search response.</returns>
        Task<ProviderOneSearchResponse> SearchAsync(ProviderOneSearchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Determines whether is available.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if available.</returns>
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
