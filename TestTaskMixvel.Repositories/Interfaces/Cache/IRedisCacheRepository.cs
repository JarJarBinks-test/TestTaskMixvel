namespace TestTaskMixvel.Repositories.Interfaces.Cache
{
    /// <summary>
    /// Interface for redis cache repository.
    /// </summary>
    public interface IRedisCacheRepository
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Result or null if cache does not exist.</returns>
        ValueTask<T?> Get<T>(string key, CancellationToken cancellationToken)
            where T : class;

        /// <summary>
        /// Sets cache by the specified key.
        /// </summary>
        /// <typeparam name="T">Type of data.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="expirationDate">Expiration date.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The set task.</returns>
        Task Set<T>(string key, T data, DateTime expirationDate, CancellationToken cancellationToken);
    }
}
