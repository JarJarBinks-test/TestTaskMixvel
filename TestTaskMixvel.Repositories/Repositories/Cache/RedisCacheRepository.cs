using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TestTaskMixvel.Repositories.Interfaces.Cache;

namespace TestTaskMixvel.Repositories.Repositories.Cache
{
    /// <summary>
    /// Redis cache repository.
    /// </summary>
    public class RedisCacheRepository : IRedisCacheRepository
    {
        readonly ILogger<RedisCacheRepository> _logger;
        readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheRepository"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="logger">The logger.</param>
        public RedisCacheRepository(IDistributedCache cache, ILogger<RedisCacheRepository> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async ValueTask<T?> Get<T>(string key, CancellationToken cancellationToken)
            where T : class
        {
            _logger.LogInformation($"{nameof(Get)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {nameof(key)}: {key}");

            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var cacheResult = await _cache.GetAsync(key, cancellationToken);
                if (cacheResult == null)
                {
                    return null;
                }
                else
                {
                    using var cacheResulStream = new MemoryStream(cacheResult);
                    return await JsonSerializer.DeserializeAsync<T>(cacheResulStream, cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Get)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {nameof(key)}: {key}. Message: {ex.Message}, Stack: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc />
        public Task Set<T>(string key, T data, DateTime expirationDate, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Set)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {nameof(key)}: {key}. {nameof(expirationDate)}: {expirationDate}");

            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                using var cacheResulStream = new MemoryStream();
                JsonSerializer.SerializeAsync(cacheResulStream, data, cancellationToken: cancellationToken);
                return _cache.SetStringAsync(key, Encoding.UTF8.GetString(cacheResulStream.ToArray()), new DistributedCacheEntryOptions() { AbsoluteExpiration = expirationDate }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Set)}. {DateTime.UtcNow}. CorrelationId: {System.Diagnostics.Activity.Current?.Id}. {nameof(key)}: {key}. {nameof(expirationDate)}: {expirationDate}. Message: {ex.Message}, Stack: {ex.StackTrace}");
                throw;
            }
        }
    }
}
