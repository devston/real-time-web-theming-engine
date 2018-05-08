namespace RealTimeThemingEngine.Web.Common.Interfaces
{
    public interface IThemeEngineService
    {
        /// <summary>
        /// Get the theme variable value by its name from the active theme.
        /// </summary>
        /// <param name="name">Name of the theme variable</param>
        /// <returns>Value of the theme variable</returns>
        string GetThemeVariableValue(string name);
    }
}
