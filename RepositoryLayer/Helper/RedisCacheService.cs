using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace RepositoryLayer.Helper
{
    /// <summary>
    /// Service for handling Redis caching operations.
    /// Provides methods to store, retrieve, and remove cached data.
    /// </summary>
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="cache">Distributed cache instance.</param>
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Stores data in the Redis cache with a specified expiration time.
        /// </summary>
        /// <typeparam name="T">Type of the data to store.</typeparam>
        /// <param name="key">Unique key for the cached item.</param>
        /// <param name="value">Data to be cached.</param>
        /// <param name="expirationMinutes">Time in minutes before the cache expires.</param>
        public void SetCache<T>(string key, T value, int expirationMinutes)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
            };

            var jsonData = JsonConvert.SerializeObject(value);
            _cache.SetString(key, jsonData, options);
        }

        /// <summary>
        /// Retrieves data from the Redis cache.
        /// </summary>
        /// <typeparam name="T">Type of the data to retrieve.</typeparam>
        /// <param name="key">Unique key of the cached item.</param>
        /// <returns>Deserialized object if found, otherwise default value.</returns>
        public T GetCache<T>(string key)
        {
            var cachedData = _cache.GetString(key);
            return cachedData != null ? JsonConvert.DeserializeObject<T>(cachedData) : default;
        }

        /// <summary>
        /// Removes a specific item from the Redis cache.
        /// </summary>
        /// <param name="key">Unique key of the cached item to remove.</param>
        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
