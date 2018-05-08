using dotless.Core.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    [DisplayName("ContrastPlugin")]
    public class ContrastPlugin : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string, Type>
            {
                { "getContrast", typeof(ContrastFunction) }
            };
        }
    }
}