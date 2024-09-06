namespace TestTaskMixvel.Repositories.Models.Provider.ProviderTwo
{
    /// <summary>
    /// Provider two search response model.
    /// </summary>
    public class ProviderTwoSearchResponse
    {
        /// <summary>
        /// Gets or sets the routes. (Mandatory. Array of routes)
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        public ProviderTwoRoute[] Routes { get; set; } = null!;
    }
}
