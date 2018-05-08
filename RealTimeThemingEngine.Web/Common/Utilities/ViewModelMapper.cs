﻿using AutoMapper;
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