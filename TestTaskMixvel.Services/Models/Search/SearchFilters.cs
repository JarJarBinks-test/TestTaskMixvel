namespace TestTaskMixvel.Services.Models.Search
{
    /// <summary>
    /// Search request filters.
    /// </summary>
    public class SearchFilters
    {
        /// <summary>
        /// Gets or sets the destination date time. (End date of route)
        /// </summary>
        /// <value>
        /// The destination date time.
        /// </value>
        public DateTime? DestinationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the maximum price. (Maximum price of route)
        /// </summary>
        /// <value>
        /// The maximum price.
        /// </value>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Gets or sets the minimum time limit. (Minimum value of timelimit for route)
        /// </summary>
        /// <value>
        /// The minimum time limit.
        /// </value>
        public DateTime? MinTimeLimit { get; set; }

        /// <summary>
        /// Gets or sets the only cached. (Forcibly search in cached data)
        /// </summary>
        /// <value>
        /// The only cached.
        /// </value>
        public bool? OnlyCached { get; set; }
    }
}
