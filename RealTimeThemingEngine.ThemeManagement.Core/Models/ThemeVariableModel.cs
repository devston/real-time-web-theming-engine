using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Models
{
    public class ThemeVariableModel
    {
        public int ThemeVariableId { get; set; }
        public int VariableTypeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public ThemeVariableCategoryModel ThemeVariableCategory { get; set; }
        public ThemeVariableTypeModel ThemeVariableType { get; set; }
        public List<ThemeVariableValueModel> ThemeVariableValues { get; set; }
    }
}
