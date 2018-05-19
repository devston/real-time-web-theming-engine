using Newtonsoft.Json;
using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Core.Models;
using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Services
{
    public class ThemeService : IThemeService
    {
        // Convert a collection of theme variable values to a json array.
        public string ConvertThemeVariableValuesToJson(IEnumerable<ThemeVariableValueModel> values)
        {
            return JsonConvert.SerializeObject(values,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
        }

        // Convert a json array to a collection of theme variable values.
        public List<ThemeVariableValueModel> ConvertJsonToThemeVariableValues(string json)
        {
            return JsonConvert.DeserializeObject<List<ThemeVariableValueModel>>(json);
        }
    }
}
