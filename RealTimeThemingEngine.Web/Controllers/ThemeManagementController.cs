using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.Web.Common.Interfaces;
using RealTimeThemingEngine.Web.Common.Utilities;
using RealTimeThemingEngine.Web.Models.Theming;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RealTimeThemingEngine.Web.Controllers
{
    public class ThemeManagementController : Controller
    {
        private readonly IThemeRepository _themeRepository;
        private readonly IThemeService _themeService;
        private readonly IImageService _imageService;
        private readonly ViewModelMapper _mapper;

        public ThemeManagementController(IThemeRepository themeRepository, IThemeService themeService, IImageService imageService, ViewModelMapper mapper)
        {
            _themeRepository = themeRepository;
            _themeService = themeService;
            _imageService = imageService;
            _mapper = mapper;
        }

        // GET: ThemeManagement
        public ActionResult Index()
        {
            var themes = _themeRepository.GetThemes();
            var vm = _mapper.Map<List<ThemeListViewModel>>(themes);
            return View(vm);
        }
    }
}