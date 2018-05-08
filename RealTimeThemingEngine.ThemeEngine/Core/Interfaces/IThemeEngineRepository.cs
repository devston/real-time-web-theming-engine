using RealTimeThemingEngine.ThemeEngine.Data.Entities;
using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeEngine.Core.Interfaces
{
    public interface IThemeEngineRepository
    {
        /// <summary>
        /// Get the current active theme variables.
        /// </summary>
        /// <returns>A collection of theme variables with the theme values.</returns>
        List<ActiveTheme> GetActiveThemeVariables();
        /// <summary>
        /// Remove an item from the cache.
        /// </summary>
        /// <param name="key">The cache key</param>
        void ClearThemeCache(string key);
    }
}
