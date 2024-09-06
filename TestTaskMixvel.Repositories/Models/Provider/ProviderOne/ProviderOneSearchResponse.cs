namespace TestTaskMixvel.Repositories.Models.Provider.ProviderOne
{
    /// <summary>
    /// Provider one search response model.
    /// </summary>
    public class ProviderOneSearchResponse
    {
        /// <summary>
        /// Gets or sets the routes. (Mandatory. Array of routes)
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        public ProviderOneRoute[] Routes { get; set; } = Array.Empty<ProviderOneRoute>();
    }
}
