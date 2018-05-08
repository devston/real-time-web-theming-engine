using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Data.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RealTimeThemingEngine.ThemeManagement.Data.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly ThemeEntitiesDBContext _context;
        private readonly IThemeEngineRepository _themeEngine;

        public ThemeRepository(IThemeEngineRepository themeEngine)
        {
            _context = new ThemeEntitiesDBContext();
            _themeEngine = themeEngine;
        }

        #region Gets

        // Get all themes.
        public IEnumerable<Theme> GetThemes()
        {
            return _context.Themes.AsNoTracking();
        }

        // Get a theme by theme id.
        public Theme GetThemeById(int id)
        {
            return _context.Themes.AsNoTracking().FirstOrDefault(x => x.ThemeId == id);
        }

        // Get a theme with all its variable values by theme id.
        public Theme GetThemeWithVariablesById(int id)
        {
            return _context.Themes.Include(x => x.ThemeVariableValues).FirstOrDefault(x => x.ThemeId == id);
        }

        // Get the active theme.
        public Theme GetActiveTheme()
        {
            return _context.Themes.AsNoTracking().FirstOrDefault(x => x.Active);
        }

        // Get theme variable values by theme id.
        public IEnumerable<ThemeVariableValue> GetThemeVariableValues(int id)
        {
            return _context.ThemeVariableValues.Include(x => x.ThemeVariable).AsNoTracking().Where(x => x.ThemeId == id).OrderBy(x => x.ThemeVariable.CategoryId).ThenBy(x => x.ThemeVariable.Order);
        }

        // Get theme variable value by value id.
        public ThemeVariableValue GetThemeVariableValueById(int id)
        {
            return _context.ThemeVariableValues.AsNoTracking().FirstOrDefault(x => x.ThemeVariableValueId == id);
        }

        // Get theme variable categories.
        public IEnumerable<ThemeVariableCategory> GetThemeVariableCategories()
        {
            return _context.ThemeVariableCategories.AsNoTracking().OrderBy(x => x.Order);
        }

        // Get theme variable types.
        public IEnumerable<ThemeVariableType> GetThemeVariableTypes()
        {
            return _context.ThemeVariableTypes.AsNoTracking();
        }

        // Get all theme variable values by variable id.
        public IEnumerable<ThemeVariableValue> GetThemeVariableValuesByVariableId(int id)
        {
            return _context.ThemeVariableValues.Include(x => x.Theme).AsNoTracking().Where(x => x.VariableId == id);
        }

        #endregion

        #region Checks

        // Check if the theme exists by theme id.
        public bool DoesThemeExist(int id)
        {
            return _context.Themes.Any(x => x.ThemeId == id);
        }

        // Check if a theme name is already in use.
        public bool IsThemeNameAlreadyInUse(int id, string name)
        {
            return _context.Themes.Any(x => x.ThemeId != id && x.Name.ToLower() == name.ToLower());
        }

        // Check if the provided theme is the active theme.
        public bool IsActiveTheme(int id)
        {
            return _context.Themes.Any(x => x.ThemeId == id && x.Active);
        }

        // Check if the provided theme is marked as read only.
        public bool IsThemeReadOnly(int id)
        {
            return _context.Themes.Any(x => x.ThemeId == id && x.ReadOnly);
        }

        // Check if the name has been changed for a theme.
        public bool HasNameBeenChanged(int id, string name)
        {
            return !_context.Themes.Any(x => x.ThemeId == id && x.Name.ToLower() == name.ToLower());
        }

        // Check if the theme variable values exist.
        public bool DoThemeVariableValuesExist(IEnumerable<int> valueIds)
        {
            return _context.ThemeVariableValues.Any(x => valueIds.Any(y => y == x.ThemeVariableValueId));
        }

        // Check if the logo is in use.
        public bool IsLogoInUse(string logoName)
        {
            return _context.ThemeVariableValues.Any(x => x.Value.ToLower() == logoName.ToLower());
        }

        #endregion

        #region Create, Update and Delete

        // Create a new theme.
        public Theme CreateTheme(Theme theme, int themeToCloneId)
        {
            // Get the theme to clone.
            var themeToClone = GetThemeWithVariablesById(themeToCloneId);

            // Copy all the theme variables to the new theme.
            foreach (var themeVariable in themeToClone.ThemeVariableValues)
            {
                theme.ThemeVariableValues.Add(new ThemeVariableValue
                {
                    ThemeVariableValueId = themeVariable.ThemeVariableValueId,
                    VariableId = themeVariable.VariableId,
                    Value = themeVariable.Value
                });
            }

            // If the theme has been marked as active, then toggle the current active theme.
            if (theme.Active)
            {
                ToggleActiveTheme();
            }

            // Add the theme and save.
            _context.Themes.Add(theme);
            SaveChanges();

            // If the theme has been marked as active, clear the active theme cache after db save.
            if (theme.Active)
            {
                ClearActiveThemeCache();
            }

            return theme;
        }

        // Save any changes to a theme.
        public void SaveThemeChanges(Theme theme)
        {
            bool isActiveTheme = false;
            var entry = _context.Entry(theme);
            entry.State = EntityState.Modified;

            // If the current theme is not the active theme and
            // has been marked as the active, then toggle the current
            // active theme.
            if (!IsActiveTheme(theme.ThemeId) && theme.Active)
            {
                ToggleActiveTheme();
                isActiveTheme = true;
            }

            SaveChanges();

            // Clear the active theme cache.
            if (isActiveTheme)
            {
                ClearActiveThemeCache();
            }
        }

        // Import a theme.
        public int ImportTheme(string name, List<ThemeVariableValue> values)
        {
            // Create a new theme.
            var theme = new Theme()
            {
                Name = name
            };

            theme.ThemeVariableValues = new List<ThemeVariableValue>();

            // Add all the values to the theme.
            foreach (var item in values)
            {
                theme.ThemeVariableValues.Add(new ThemeVariableValue()
                {
                    Value = item.Value,
                    VariableId = item.VariableId
                });
            }

            // Add the theme to the context.
            _context.Themes.Add(theme);

            // Save the theme to the db.
            SaveChanges();

            // Return the theme id.
            return theme.ThemeId;
        }

        // Update the theme variable values.
        public void UpdateThemeVariableValues(IEnumerable<ThemeVariableValue> themeVariables)
        {
            if (themeVariables != null && themeVariables.Any())
            {
                int themeId = 0;

                foreach (var item in themeVariables)
                {
                    var originalVariable = GetThemeVariableValueById(item.ThemeVariableValueId);
                    originalVariable.Value = item.Value;
                    themeId = originalVariable.ThemeId;
                    _context.Entry(originalVariable).State = EntityState.Modified;
                }

                SaveChanges();

                // Clear the theme cache if the theme was active.
                if (IsActiveTheme(themeId))
                {
                    ClearActiveThemeCache();
                }
            }
        }

        // Delete the theme and all its variable values.
        public void DeleteTheme(int id)
        {
            Theme theme = GetThemeWithVariablesById(id);

            // Delete all the theme variable values attached to the theme.
            _context.ThemeVariableValues.RemoveRange(theme.ThemeVariableValues);

            // Delete the theme.
            _context.Themes.Remove(theme);

            // Save changes.
            SaveChanges();
        }

        #endregion

        #region Helper methods

        // Mark the current active theme as not active.
        private void ToggleActiveTheme()
        {
            var theme = GetActiveTheme();
            theme.Active = false;
            _context.Entry(theme).State = EntityState.Modified;
        }

        // Clear the active theme cache.
        private void ClearActiveThemeCache()
        {
            _themeEngine.ClearThemeCache("ActiveTheme");
        }

        // Utility class to save entity changes.
        private void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}
