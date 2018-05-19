using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeThemingEngine.ThemeEngine.Data.Repositories
{
    public class ThemeEngineRepository : IThemeEngineRepository
    {
        private readonly ThemeEngineDbContext _context;

        public ThemeEngineRepository()
        {
            _context = new ThemeEngineDbContext();
        }

        // Get the active theme variables.
        public Dictionary<string, string> GetActiveThemeVariables()
        {
            return _context.ActiveThemes.AsNoTracking().ToDictionary(x => x.Name, x => x.Value);
        }

        public void ClearThemeCache(string key)
        {
            throw new NotImplementedException();
        }
    }
}
