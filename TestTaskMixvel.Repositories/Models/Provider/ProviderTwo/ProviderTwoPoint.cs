namespace TestTaskMixvel.Repositories.Models.Provider.ProviderTwo
{
    /// <summary>
    /// Provider two point response model.
    /// </summary>
    public class ProviderTwoPoint
    {
        /// <summary>
        /// Gets or sets the point. (Mandatory. Name of point, e.g. Moscow\Sochi)
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public string Point { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date. (Mandatory. Date for point in Route, e.g. Point = Moscow, Date = 2023-01-01 15-00-00)
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
    }
}
