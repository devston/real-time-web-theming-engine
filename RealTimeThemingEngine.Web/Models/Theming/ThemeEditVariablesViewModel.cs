using System.Collections.Generic;

namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class ThemeEditVariablesViewModel
    {
        public int ThemeId { get; set; }
        public List<ThemeVariableCategoryViewModel> VariableCategories { get; set; }
        public List<ThemeVariableValueViewModel> Variables { get; set; }
    }
}