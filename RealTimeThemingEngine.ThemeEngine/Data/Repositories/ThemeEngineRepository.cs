using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using RealTimeThemingEngine.ThemeEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeThemingEngine.ThemeEngine.Data.Repositories
{
    public class ThemeEngineRepository : IThemeEngineRepository
    {
        private readonly ThemeEngineEntitiesDBContext _context;

        public ThemeEngineRepository()
        {
            _context = new ThemeEngineEntitiesDBContext();
        }

        // Get the active theme variables.
        public List<ActiveTheme> GetActiveThemeVariables()
        {
            return _context.ActiveThemes.AsNoTracking().ToList();
        }

        public void ClearThemeCache(string key)
        {
            throw new NotImplementedException();
        }
    }
}
