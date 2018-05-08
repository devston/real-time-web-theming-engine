using dotless.Core.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    [DisplayName("ColourPlugin")]
    public class ColourPlugin : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string, Type>
            {
                { "getColour", typeof(ColourFunction) }
            };
        }
    }
}