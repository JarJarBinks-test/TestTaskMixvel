namespace TestTaskMixvel.Services.Models.Search
{
    /// <summary>
    /// Search response route model.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Gets the identifier. (Mandatory. Identifier of the whole route)
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the origin. (Mandatory. Start point of route)
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public string Origin { get; set; } = null!;

        /// <summary>
        /// Gets or sets the destination. (Mandatory. End point of route)
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public string Destination { get; set; } = null!;

        /// <summary>
        /// Gets or sets the origin date time. (Mandatory. Start date of route)
        /// </summary>
        /// <value>
        /// The origin date time.
        /// </value>
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// Gets or sets the destination date time. (Mandatory. End date of route)
        /// </summary>
        /// <value>
        /// The destination date time.
        /// </value>
        public DateTime DestinationDateTime { get; set; }

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
