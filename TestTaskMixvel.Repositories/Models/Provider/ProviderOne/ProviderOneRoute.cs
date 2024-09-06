namespace TestTaskMixvel.Repositories.Models.Provider.ProviderOne
{
    /// <summary>
    /// Provider one route response model.
    /// </summary>
    public class ProviderOneRoute
    {
        /// <summary>
        /// Gets or sets from. (Mandatory. Start point of route)
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string From { get; set; } = null!;

        /// <summary>
        /// Gets or sets to. (Mandatory. End point of route)
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
        /// Gets or sets the date to. (Mandatory. End date of route)
        /// </summary>
        /// <value>
        /// The date to.
        /// </value>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the price. (Mandatory. Price of route)
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the time limit. (Mandatory. Timelimit. After it expires, route became not actual)
        /// </summary>
        /// <value>
        /// The time limit.
        /// </value>
        public DateTime TimeLimit { get; set; }
    }
}
