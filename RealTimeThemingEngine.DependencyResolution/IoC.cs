using RealTimeThemingEngine.ThemeEngine.Core.Interfaces;
using RealTimeThemingEngine.ThemeEngine.Data.Repositories;
using RealTimeThemingEngine.ThemeManagement.Core.Interfaces;
using RealTimeThemingEngine.ThemeManagement.Core.Services;
using RealTimeThemingEngine.ThemeManagement.Data.Repositories;
using StructureMap;
using System;

namespace RealTimeThemingEngine.DependencyResolution
{
    public static partial class IoC
    {
        public static IContainer Initialize(Action<ConfigurationExpression> moreInitialization)
        {
            return new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();

                    x.For<IThemeRepository>().Use<ThemeRepository>();
                    x.For<IThemeService>().Use<ThemeService>();
                    x.For<IThemeEngineRepository>().Use<CachedThemeEngineRepository>()
                    .Ctor<IThemeEngineRepository>().Is<ThemeEngineRepository>();
                });

                moreInitialization?.Invoke(x);
            });
        }
    }
}
