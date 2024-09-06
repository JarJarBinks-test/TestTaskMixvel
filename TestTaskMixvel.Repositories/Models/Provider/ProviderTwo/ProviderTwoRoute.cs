namespace TestTaskMixvel.Repositories.Models.Provider.ProviderTwo
{
    /// <summary>
    /// Provider two route repsponse model.
    /// </summary>
    public class ProviderTwoRoute
    {
        /// <summary>
        /// Gets or sets the departure. (Mandatory. Start point of route)
        /// </summary>
        /// <value>
        /// The departure.
        /// </value>
        public ProviderTwoPoint Departure { get; set; } = null!;

        /// <summary>
        /// Gets or sets the arrival. (Mandatory. End point of route)
        /// </summary>
        /// <value>
        /// The arrival.
        /// </value>
        public ProviderTwoPoint Arrival { get; set; } = null!;

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
