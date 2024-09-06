namespace TestTaskMixvel.Repositories.Models.Provider.ProviderTwo
{
    /// <summary>
    /// Provider two search request model.
    /// </summary>
    public class ProviderTwoSearchRequest
    {
        /// <summary>
        /// Gets or sets the departure. (Mandatory. Start point of route, e.g. Moscow )
        /// </summary>
        /// <value>
        /// The departure.
        /// </value>
        public string Departure { get; set; } = null!;

        /// <summary>
        /// Gets or sets the arrival. (Mandatory. End point of route, e.g. Sochi)
        /// </summary>
        /// <value>
        /// The arrival.
        /// </value>
        public string Arrival { get; set; } = null!;

        /// <summary>
        /// Gets or sets the departure date. (Mandatory. Start date of route)
        /// </summary>
        /// <value>
        /// The departure date.
        /// </value>
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum time limit. (Optional. Minimum value of timelimit for route)
        /// </summary>
        /// <value>
        /// The minimum time limit.
        /// </value>
        public DateTime? MinTimeLimit { get; set; } = null!;
    }
}