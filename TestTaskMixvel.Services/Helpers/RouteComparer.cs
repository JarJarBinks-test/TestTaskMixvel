using System.Diagnostics.CodeAnalysis;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.Services.Helpers
{
    // TODO: Not used. Nedd remove.
    internal class RouteComparer : IEqualityComparer<Route>
    {
        public bool Equals(Route? x, Route? y)
        {
            return x?.TimeLimit == y?.TimeLimit &&
               x?.Origin == y?.Origin &&
               x?.OriginDateTime == y?.OriginDateTime &&
               x?.Destination == y?.Destination &&
               x?.DestinationDateTime == y?.DestinationDateTime &&
               x?.Price == y?.Price;
        }

        public int GetHashCode([DisallowNull] Route obj)
        {
            throw new NotImplementedException();
        }
    }
}
