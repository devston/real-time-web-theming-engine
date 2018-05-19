using RealTimeThemingEngine.ThemeManagement.Core.Models;
using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Interfaces
{
    public interface IThemeService
    {
        /// <summary>
        /// Convert a collection of theme variable values to a json array.
        /// </summary>
        /// <param name="values">Collection of theme variable values</param>
        /// <returns></returns>
        string ConvertThemeVariableValuesToJson(IEnumerable<ThemeVariableValueModel> values);

        /// <summary>
        /// Convert a json array to a collection of theme variable values.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        List<ThemeVariableValueModel> ConvertJsonToThemeVariableValues(string json);
    }
}
