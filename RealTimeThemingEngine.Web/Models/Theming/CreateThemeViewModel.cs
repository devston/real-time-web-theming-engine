using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class CreateThemeViewModel : ThemeViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public int SelectedThemeId { get; set; }

        [DisplayName("Select a theme to clone")]
        [Required(ErrorMessage = "{0} is required")]
        public string SelectedThemeName { get; set; }
    }
}