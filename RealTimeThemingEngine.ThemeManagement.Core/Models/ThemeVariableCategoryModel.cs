using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Models
{
    public class ThemeVariableCategoryModel
    {
        public int ThemeVariableCategoryId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public List<ThemeVariableModel> ThemeVariables { get; set; }
    }
}
