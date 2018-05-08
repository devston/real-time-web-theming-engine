using Newtonsoft.Json;
using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Data.Entities;
using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Services
{
    public class ThemeService : IThemeService
    {
        // Convert a collection of theme variable values to a json array.
        public string ConvertThemeVariableValuesToJson(IEnumerable<ThemeVariableValue> values)
        {
            return JsonConvert.SerializeObject(values,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
        }

        // Convert a json array to a collection of theme variable values.
        public List<ThemeVariableValue> ConvertJsonToThemeVariableValues(string json)
        {
            return JsonConvert.DeserializeObject<List<ThemeVariableValue>>(json);
        }
    }
}
