using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;
using RealTimeThemingEngine.Web.Common.Interfaces;
using System.Linq;
using System.Text.RegularExpressions;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    public class ColourFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count(), this, Location);
            Guard.ExpectNode<Keyword>(Arguments[0], this, Arguments[0].Location);
            var variableName = Arguments[0] as Keyword;
            string colourCode;

            // Unfortunately cannot use DI properly as this is an external plugin that has not implemented this feature.
            // Manually get the instance in a nested container to get around this.
            var container = RealTimeThemingEngine.DependencyResolution.IoC.GetMainContainer();

            using (var nested = container.GetNestedContainer())
            {
                IThemeEngineService themeService = nested.GetInstance<IThemeEngineService>();
                colourCode = themeService.GetThemeVariableValue(variableName.Value);
            }

            // Replace the code if the colour is not a valid hex to prevent an exception.
            if (!Regex.Match(colourCode, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
            {
                colourCode = "#000";
            }

            var colour = System.Drawing.ColorTranslator.FromHtml(colourCode);
            return new Color(colour.R, colour.G, colour.B);
        }
    }
}