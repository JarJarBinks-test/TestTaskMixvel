namespace TestTaskMixvel.Services.Models.Search
{
    /// <summary>
    /// Search response model.
    /// </summary>
    public class SearchResponse
    {
        /// <summary>
        /// Gets or sets the routes. (Mandatory. Array of routes)
        /// </summary>
        /// <value>
        /// The routes.
        /// </value>
        public Route[] Routes { get; set; } = Array.Empty<Route>();

        /// <summary>
        /// Gets or sets the minimum price. (Mandatory. The cheapest route)
        /// </summary>
        /// <value>
        /// The minimum price.
        /// </value>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Gets or sets the maximum price. (Mandatory. Most expensive route)
        /// </summary>
        /// <value>
        /// The maximum price.
        /// </value>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Gets or sets the minimum minutes route. (Mandatory. The fastest route)
        /// </summary>
        /// <value>
        /// The minimum minutes route.
        /// </value>
        public int MinMinutesRoute { get; set; }

        /// <summary>
        /// Gets or sets the maximum minutes route. (Mandatory. The longest route)
        /// </summary>
        /// <value>
        /// The maximum minutes route.
        /// </value>
        public int MaxMinutesRoute { get; set; }
    }
}
