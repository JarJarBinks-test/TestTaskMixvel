using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TestTaskMixvel.Services.Models.Search
{
    /// <summary>
    /// Search request model.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <value>
        /// The unique key.
        /// </value>
        [IgnoreDataMember]
        [JsonIgnore]
        public String UniqueKey
        {
            get
            {
                return $"{Origin}_{Destination}_{OriginDateTime}_{Filters?.MinTimeLimit}_{Filters?.MaxPrice}_{Filters?.DestinationDateTime}";
            }
        }

        /// <summary>
        /// Gets or sets the origin. (Start point of route, e.g. Moscow)
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        [Required(AllowEmptyStrings = false)]
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the destination. (End point of route, e.g. Sochi)
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        [Required(AllowEmptyStrings = false)]
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the origin date time. (Start date of route)
        /// </summary>
        /// <value>
        /// The origin date time.
        /// </value>
        [Required]
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public SearchFilters? Filters { get; set; }
    }
}
