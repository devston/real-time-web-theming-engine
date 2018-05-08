using RealTimeThemingEngine.ThemeManagement.Data.Entities;
using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Interfaces
{
    public interface IThemeRepository
    {
        #region Gets

        /// <summary>
        /// Get all themes.
        /// </summary>
        /// <returns>A collection of themes</returns>
        IEnumerable<Theme> GetThemes();
        /// <summary>
        /// Get a theme by a theme id.
        /// </summary>
        /// <param name="id">Theme id</param>
        /// <returns>A theme</returns>
        Theme GetThemeById(int id);
        /// <summary>
        /// Get a theme with all its variable values by theme id.
        /// </summary>
        /// <param name="id">The theme id</param>
        /// <returns></returns>
        Theme GetThemeWithVariablesById(int id);
        /// <summary>
        /// Get the active theme.
        /// </summary>
        /// <returns></returns>
        Theme GetActiveTheme();
        /// <summary>
        /// Get theme variable values by theme id.
        /// </summary>
        /// <param name="id">Theme id</param>
        /// <returns>A collection of theme variable values</returns>
        IEnumerable<ThemeVariableValue> GetThemeVariableValues(int id);
        /// <summary>
        /// Get theme variable value by value id.
        /// </summary>
        /// <param name="id">Theme variable value id</param>
        /// <returns>A theme variable value</returns>
        ThemeVariableValue GetThemeVariableValueById(int id);
        /// <summary>
        /// Get theme variable categories.
        /// </summary>
        /// <returns>A collection of theme variable categories</returns>
        IEnumerable<ThemeVariableCategory> GetThemeVariableCategories();
        /// <summary>
        /// Get theme variable types.
        /// </summary>
        /// <returns>A collection of theme variable types</returns>
        IEnumerable<ThemeVariableType> GetThemeVariableTypes();
        /// <summary>
        /// Get all theme variable values by theme variable id.
        /// </summary>
        /// <param name="id">Theme variable id</param>
        /// <returns></returns>
        IEnumerable<ThemeVariableValue> GetThemeVariableValuesByVariableId(int id);

        #endregion

        #region Checks

        /// <summary>
        /// Check if the theme exists by theme id.
        /// </summary>
        /// <param name="id">The theme id</param>
        /// <returns></returns>
        bool DoesThemeExist(int id);
        /// <summary>
        /// Check if the theme name is already in use.
        /// </summary>
        /// <param name="id">Theme id of the theme having a name change</param>
        /// <param name="name">Theme name</param>
        /// <returns></returns>
        bool IsThemeNameAlreadyInUse(int id, string name);
        /// <summary>
        /// Check if the provided theme is the current active theme.
        /// </summary>
        /// <param name="id">Theme id</param>
        /// <returns></returns>
        bool IsActiveTheme(int id);
        /// <summary>
        /// Check if the provided theme is marked as read only.
        /// </summary>
        /// <param name="id">Theme id</param>
        /// <returns></returns>
        bool IsThemeReadOnly(int id);
        /// <summary>
        /// Check if the theme name has been changed.
        /// </summary>
        /// <param name="id">Theme id</param>
        /// <param name="name">Theme name</param>
        /// <returns></returns>
        bool HasNameBeenChanged(int id, string name);
        /// <summary>
        /// Check if the theme variable values exist.
        /// </summary>
        /// <param name="themeVariables"></param>
        /// <returns></returns>
        bool DoThemeVariableValuesExist(IEnumerable<int> themeVariables);
        /// <summary>
        /// Check if the logo is in use.
        /// </summary>
        /// <param name="logoName"></param>
        /// <returns></returns>
        bool IsLogoInUse(string logoName);

        #endregion

        #region Create, Update and Delete

        /// <summary>
        /// Create a new theme.
        /// </summary>
        /// <param name="theme">The theme to create</param>
        /// <param name="themeToCloneId">The theme id of the theme to clone variables from</param>
        /// <returns>The created theme</returns>
        Theme CreateTheme(Theme theme, int themeToCloneId);
        /// <summary>
        /// Save any changes to a theme (not including theme variables).
        /// </summary>
        /// <param name="theme"></param>
        void SaveThemeChanges(Theme theme);
        /// <summary>
        /// Import a theme.
        /// </summary>
        /// <param name="name">Name for the new theme</param>
        /// <param name="values">Imported theme variable values</param>
        /// <returns>The created theme id</returns>
        int ImportTheme(string name, List<ThemeVariableValue> values);
        /// <summary>
        /// Update the theme variable values.
        /// </summary>
        /// <param name="themeVariables">Collection of theme variables</param>
        void UpdateThemeVariableValues(IEnumerable<ThemeVariableValue> themeVariables);
        /// <summary>
        /// Delete a theme and all its theme variable values.
        /// </summary>
        /// <param name="id">Theme id</param>
        void DeleteTheme(int id);

        #endregion
    }
}
