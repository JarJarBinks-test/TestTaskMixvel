namespace TestTaskMixvel.Repositories.Models.Provider.ProviderOne
{
    /// <summary>
    /// Provider one search request model.
    /// </summary>
    public class ProviderOneSearchRequest
    {
        /// <summary>
        /// Gets or sets from. (Mandatory. Start point of route, e.g. Moscow )
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string From { get; set; } = null!;

        /// <summary>
        /// Gets or sets to. (Mandatory. End point of route, e.g. Sochi)
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date from. (Mandatory. Start date of route)
        /// </summary>
        /// <value>
        /// The date from.
        /// </value>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to. (Optional. End date of route)
        /// </summary>
        /// <value>
        /// The date to.
        /// </value>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets the maximum price. (Optional. Maximum price of route)
        /// </summary>
        /// <value>
        /// The maximum price.
        /// </value>
        public decimal? MaxPrice { get; set; }
    }
}
