using dotless.Core.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    [DisplayName("SizePlugin")]
    public class SizePlugin : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string, Type>
            {
                { "getSize", typeof(SizeFunction) }
            };
        }
    }
}