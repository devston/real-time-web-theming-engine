﻿using dotless.Core.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    [DisplayName("CalculateRemPlugin")]
    public class CalculateRemPlugin : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string, Type>
            {
                { "calculateRem", typeof(CalculateRemFunction) }
            };
        }
    }
}