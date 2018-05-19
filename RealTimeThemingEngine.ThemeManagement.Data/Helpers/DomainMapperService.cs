using AutoMapper;
using RealTimeThemingEngine.ThemeManagement.Core.Models;
using RealTimeThemingEngine.ThemeManagement.Data.Entities;
using System;

namespace RealTimeThemingEngine.ThemeManagement.Data.Helpers
{
    public class DomainMapperService
    {
        private static IMapper _mapper;

        static DomainMapperService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Theme, ThemeModel>()
                   .ReverseMap();

                cfg.CreateMap<ThemeVariable, ThemeVariableModel>()
                   .ReverseMap();

                cfg.CreateMap<ThemeVariableCategory, ThemeVariableCategoryModel>()
                   .ReverseMap();

                cfg.CreateMap<ThemeVariableType, ThemeVariableTypeModel>()
                   .ReverseMap();

                cfg.CreateMap<ThemeVariableValue, ThemeVariableValueModel>()
                   .ReverseMap();
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
