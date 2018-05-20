using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RealTimeThemingEngine.Web.Models.Theming
{
    public class ImportThemeViewModel
    {
        [Required(ErrorMessage = "Enter a {0}")]
        [StringLength(50, ErrorMessage = "{0} should be {1} characters or less")]
        public string Name { get; set; }

        [DisplayName("Choose file")]
        [Required(ErrorMessage = "Select a theme to import")]
        public HttpPostedFileBase FileToUse { get; set; }
    }
}