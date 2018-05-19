using AutoMapper;
using RealTimeThemingEngine.ThemeManagement.Core.Models;
using RealTimeThemingEngine.Web.Models.Theming;
using System;

namespace RealTimeThemingEngine.Web.Common.Utilities
{
    public class ViewModelMapper
    {
        private static IMapper _mapper;

        static ViewModelMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ThemeModel, ThemeViewModel>()
                   .ReverseMap();

                cfg.CreateMap<ThemeModel, ThemeListViewModel>()
                   .ForMember(x => x.Active, opt => opt.MapFrom(z => z.Active ? "Yes" : "No"))
                   .ForMember(x => x.ReadOnly, opt => opt.MapFrom(z => z.ReadOnly ? "Yes" : "No"));
            });

            _mapper = config.CreateMapper();

            try
            {
                _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T Map<T>(object classToBeMapped)
        {
            return _mapper.Map<T>(classToBeMapped);
        }
    }
}