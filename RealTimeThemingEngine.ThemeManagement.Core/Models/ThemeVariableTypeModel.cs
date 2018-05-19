using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Models
{
    public class ThemeVariableTypeModel
    {
        public int ThemeVariableTypeId { get; set; }
        public string Name { get; set; }

        public List<ThemeVariableModel> ThemeVariables { get; set; }
    }
}
