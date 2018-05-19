namespace RealTimeThemingEngine.ThemeManagement.Core.Models
{
    public class ThemeVariableValueModel
    {
        public int ThemeVariableValueId { get; set; }
        public int ThemeId { get; set; }
        public int VariableId { get; set; }
        public string Value { get; set; }

        public virtual ThemeModel Theme { get; set; }
        public virtual ThemeVariableModel ThemeVariable { get; set; }
    }
}
