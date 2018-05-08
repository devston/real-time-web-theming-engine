using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using RealTimeThemingEngine.ThemeEngine.Data.Entities;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace RealTimeThemingEngine.ThemeEngine.Data.Repositories
{
    public class CachedThemeEngineRepository : IThemeEngineRepository
    {
        private readonly IThemeEngineRepository _themeRepository;

        public CachedThemeEngineRepository(IThemeEngineRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        private static readonly object CacheLockObject = new object();

        // Add the active theme to the cache.
        public List<ActiveTheme> GetActiveThemeVariables()
        {
            MemoryCache cache = MemoryCache.Default;
            string cacheKey = "ActiveTheme";
            var result = cache[cacheKey] as List<ActiveTheme>;

            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = cache[cacheKey] as List<ActiveTheme>;

                    if (result == null)
                    {
                        result = _themeRepository.GetActiveThemeVariables();
                        var cacheItem = new CacheItem(cacheKey, result);
                        var itemPolicy = new CacheItemPolicy(); // Use the default policy for theming.
                        cache.Add(cacheItem, itemPolicy);
                    }
                }
            }

            return result;
        }

        // Remove an item from the cache.
        public void ClearThemeCache(string key)
        {
            MemoryCache cache = MemoryCache.Default;

            lock (CacheLockObject)
            {
                cache.Remove(key);
            }
        }
    }
}
