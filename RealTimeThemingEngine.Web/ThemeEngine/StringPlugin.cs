using dotless.Core.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    [DisplayName("StringPlugin")]
    public class StringPlugin : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string, Type>
            {
                { "getString", typeof(StringFunction) }
            };
        }
    }
}