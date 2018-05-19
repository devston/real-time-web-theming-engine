using System.ComponentModel;

namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class ThemeListViewModel
    {
        public int ThemeId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Read only?")]
        public string ReadOnly { get; set; }

        [DisplayName("Active?")]
        public string Active { get; set; }
    }
}