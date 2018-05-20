using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Core.Models;
using RealTimeThemingEngine.Web.Common.Classes;
using RealTimeThemingEngine.Web.Common.Utilities;
using RealTimeThemingEngine.Web.Models.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RealTimeThemingEngine.Web.Controllers
{
    public class ThemeManagementController : Controller
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IThemeService _themeService;
        private readonly ViewModelMapper _mapper;

        public ThemeManagementController(IThemeRepository themeRepository, IThemeService themeService, ViewModelMapper mapper)
        {
            _themeRepository = themeRepository;
            _themeService = themeService;
            _mapper = mapper;
        }

        #region Gets

        // GET: ThemeManagement
        public ActionResult Index()
        {
            var themes = _themeRepository.GetThemes();
            var vm = _mapper.Map<List<ThemeListViewModel>>(themes);
            return View(vm);
        }

        // Create theme.
        public ActionResult CreateTheme()
        {
            var theme = _themeRepository.GetActiveTheme();
            var vm = new CreateThemeViewModel
            {
                SelectedThemeId = theme.ThemeId,
                SelectedThemeName = theme.Name
            };
            return View(vm);
        }

        // Edit theme.
        public ActionResult EditTheme(int id)
        {
            var theme = _themeRepository.GetThemeById(id);
            var vm = _mapper.Map<ThemeViewModel>(theme);
            return View(vm);
        }

        // Edit theme variables.
        public ActionResult EditThemeVariables(int id)
        {
            // Get the theme variables.
            var themeVariables = _themeRepository.GetThemeVariableValues(id);

            // Get the variable categories.
            var themeVariableCategories = _themeRepository.GetThemeVariableCategories();

            // Map them into view models.
            var variablesVm = _mapper.Map<List<ThemeVariableValueViewModel>>(themeVariables);
            var categoriesVm = _mapper.Map<List<ThemeVariableCategoryViewModel>>(themeVariableCategories);

            // Put the two view models in a wrapper view model and return the view.
            var vm = new ThemeEditVariablesViewModel
            {
                ThemeId = id,
                VariableCategories = categoriesVm,
                Variables = variablesVm
            };
            return View(vm);
        }

        // Validate the theme is ok for exporting.
        public ActionResult ValidateExportTheme(int id)
        {
            // Check if the theme exists.
            if (!_themeRepository.DoesThemeExist(id))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Could not find the theme.", "");
            }

            return Json(new { themeId = id }, JsonRequestBehavior.AllowGet);
        }

        // Export theme to a file.
        public FileResult ExportTheme(int id)
        {
            var values = _themeRepository.GetThemeVariableValues(id);
            var json = _themeService.ConvertThemeVariableValuesToJson(values);
            byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(json);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "theme.json");
        }

        // Import theme view.
        public ActionResult ImportTheme()
        {
            var vm = new ImportThemeViewModel();
            return View(vm);
        }

        // Confirm delete theme modal.
        public ActionResult _ConfirmDeleteTheme(int id)
        {
            var theme = _themeRepository.GetThemeById(id);
            var vm = _mapper.Map<ThemeViewModel>(theme);
            return PartialView(vm);
        }

        #endregion

        #region Posts

        // Create a new theme.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewTheme(CreateThemeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult(ModelState);
            }

            // Check if the theme name is already in use.
            if (_themeRepository.IsThemeNameAlreadyInUse(vm.ThemeId, vm.Name))
            {
                return SiteErrorHandler.GetBadRequestActionResult($"The theme name {vm.Name} is already in use.", nameof(vm.Name));
            }

            var theme = _mapper.Map<ThemeModel>(vm);
            int id = _themeRepository.CreateTheme(theme, vm.SelectedThemeId);

            return Json(new { message = "<strong>Success</strong>: The theme has been created.", themeId = id });
        }

        // Import theme.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportThemeFromFile(ImportThemeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult(ModelState);
            }

            // Check if the theme name is already in use.
            if (_themeRepository.IsThemeNameAlreadyInUse(0, vm.Name))
            {
                return SiteErrorHandler.GetBadRequestActionResult($"The theme name {vm.Name} is already in use.", nameof(vm.Name));
            }

            // Check if the file type is correct.
            if (vm.FileToUse.ContentType != "application/json")
            {
                return SiteErrorHandler.GetBadRequestActionResult("File must be .json", "");
            }

            // Check if the file has any content.
            if (vm.FileToUse.ContentLength == 0)
            {
                return SiteErrorHandler.GetBadRequestActionResult("File is empty", "");
            }

            List<ThemeVariableValueModel> variables;

            // Read the file and convert the json to a theme variable value collection.
            try
            {
                var b = new System.IO.BinaryReader(vm.FileToUse.InputStream);
                byte[] binData = b.ReadBytes(vm.FileToUse.ContentLength);
                string result = System.Text.Encoding.UTF8.GetString(binData);
                variables = _themeService.ConvertJsonToThemeVariableValues(result);
            }
            catch (Exception ex)
            {
                return SiteErrorHandler.GetBadRequestActionResult(ex.Message, "");
            }

            // Check if there are any values inside the file.
            if (variables == null || !variables.Any())
            {
                return SiteErrorHandler.GetBadRequestActionResult("Imported theme did not have any values", "");
            }

            // Import the theme.
            int themeId = _themeRepository.ImportTheme(vm.Name, variables);

            return Json(new { message = "<strong>Success</strong>: Theme has been created.", themeId });
        }

        // Save theme changes.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveThemeChanges(ThemeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult(ModelState);
            }

            // Check if the theme exists.
            if (!_themeRepository.DoesThemeExist(vm.ThemeId))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Could not find the theme.", "");
            }

            // Check if the theme name is already in use.
            if (_themeRepository.IsThemeNameAlreadyInUse(vm.ThemeId, vm.Name))
            {
                return SiteErrorHandler.GetBadRequestActionResult($"The theme name {vm.Name} is already in use.", nameof(vm.Name));
            }

            // Check if the theme is a system theme and has been modified.
            if (_themeRepository.IsThemeReadOnly(vm.ThemeId) && _themeRepository.HasNameBeenChanged(vm.ThemeId, vm.Name))
            {
                return SiteErrorHandler.GetBadRequestActionResult($"This is a system theme and cannot be modified.", "");
            }

            var theme = _mapper.Map<ThemeModel>(vm);
            _themeRepository.SaveThemeChanges(theme);

            return Json(new { message = "<strong>Success</strong>: The theme has been updated.", themeId = vm.ThemeId });
        }

        // Save theme variable values.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveThemeVariables(ThemeEditVariablesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult(ModelState);
            }

            // Check if the theme exists.
            if (!_themeRepository.DoesThemeExist(vm.ThemeId))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Could not find the theme.", "");
            }

            // Map the variables to a readable model.
            var themeVariables = _mapper.Map<List<ThemeVariableValueModel>>(vm.Variables);
            IEnumerable<int> themeVariableValueIds = themeVariables.Select(x => x.ThemeVariableValueId);

            // Check if the theme variable values exist.
            if (!_themeRepository.DoThemeVariableValuesExist(themeVariableValueIds))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Could not find the theme variable.", "");
            }

            // Update the theme variable values.
            _themeRepository.UpdateThemeVariableValues(themeVariables);

            return Json(new { message = "<strong>Success</strong>: The theme variables have been updated.", themeId = vm.ThemeId });
        }

        // Delete theme.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTheme(int id)
        {
            // Check if the theme exists.
            if (!_themeRepository.DoesThemeExist(id))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Could not find the theme.", "");
            }

            // Check if the theme is a system theme.
            if (_themeRepository.IsThemeReadOnly(id))
            {
                return SiteErrorHandler.GetBadRequestActionResult($"This is a system theme and cannot be deleted.", "");
            }

            // Check if the theme is in use.
            if (_themeRepository.IsActiveTheme(id))
            {
                return SiteErrorHandler.GetBadRequestActionResult("Cannot delete the theme as it is currently in use.", "");
            }

            // Delete the theme.
            _themeRepository.DeleteTheme(id);

            return Json(new { message = "<strong>Success</strong>: The theme has been deleted." });
        }

        #endregion
    }
}