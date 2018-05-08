namespace RealTimeThemingEngine.ThemeManagement.Data.Entities
{
    public class ThemeVariableValue
    {
        public int ThemeVariableValueId { get; set; }
        public int ThemeId { get; set; }
        public int VariableId { get; set; }
        public string Value { get; set; }

        public virtual Theme Theme { get; set; }
        public virtual ThemeVariable ThemeVariable { get; set; }
    }
}
