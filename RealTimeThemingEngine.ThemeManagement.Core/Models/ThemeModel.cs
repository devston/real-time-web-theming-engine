using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Core.Models
{
    public class ThemeModel
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool ReadOnly { get; set; }
        public List<ThemeVariableValueModel> ThemeVariableValues { get; set; }
    }
}
