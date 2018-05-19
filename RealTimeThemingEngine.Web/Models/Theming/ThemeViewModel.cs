using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class ThemeViewModel
    {
        public int ThemeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should be {1} characters or less")]
        public string Name { get; set; }

        [DisplayName("Default?")]
        public bool Default { get; set; }

        [DisplayName("Read only?")]
        public bool ReadOnly { get; set; }
    }
}