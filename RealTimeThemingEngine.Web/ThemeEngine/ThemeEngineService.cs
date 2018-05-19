using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using RealTimeThemingEngine.Web.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    public class ThemeEngineService : IThemeEngineService
    {
        public readonly IThemeEngineRepository _themeEngine;

        public ThemeEngineService(IThemeEngineRepository themeEngine)
        {
            _themeEngine = themeEngine;
        }

        // Get the theme variable value by its name from the active theme.
        public string GetThemeVariableValue(string name)
        {
            var themeVariables = GetActiveThemeVariables();
            var value = themeVariables.Where(x => x.Key == name).Select(x => x.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            return value;
        }

        private Dictionary<string, string> GetActiveThemeVariables()
        {
            return _themeEngine.GetActiveThemeVariables();
        }
    }
}