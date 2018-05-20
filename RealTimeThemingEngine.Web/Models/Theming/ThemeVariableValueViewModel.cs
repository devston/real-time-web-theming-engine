namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class ThemeVariableValueViewModel
    {
        public int ThemeVariableValueId { get; set; }
        public int ThemeId { get; set; }
        public int VariableId { get; set; }
        public int TypeId { get; set; }
        public string Value { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}