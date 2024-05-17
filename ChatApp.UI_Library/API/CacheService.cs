using ChatApp.UI_Library.API.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChatApp.UI_Library.API
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string userId, string cacheKey)
        {
            var userCacheKey = $"user_{userId}_{cacheKey}";

            return _memoryCache.Get<T>(userCacheKey);
        }

        public async Task<T> GetOrSetAsync<T>(string userId, string cacheKey, Func<Task<T>> func) where T : class
        {
            if (string.IsNullOrEmpty(userId))
                return null; 

            var userCacheKey = $"user_{userId}_{cacheKey}";

            var item = _memoryCache.Get<T>(userCacheKey);

            if(item == null)
            {
                var cacheItem = await func.Invoke();

                item = cacheItem;

                _memoryCache.Set(userCacheKey, cacheItem, DateTimeOffset.UtcNow.AddMinutes(10));
            }

            return item;
        }

        public void Set(string userId, string cacheKey, object value)
        {
            var userCacheKey = $"user_{userId}_{cacheKey}";

            _memoryCache.Set(userCacheKey, value);
        }
    }
}
