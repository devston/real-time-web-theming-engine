using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;
using RealTimeThemingEngine.Web.Common.Interfaces;
using System.Linq;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    public class StringFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count(), this, Location);
            Guard.ExpectNode<Keyword>(Arguments[0], this, Arguments[0].Location);
            var variableName = Arguments[0] as Keyword;
            string passedString;

            // Unfortunately cannot use DI properly as this is an external plugin that has not implemented this feature.
            // Manually get the instance in a nested container to get around this.
            var container = RealTimeThemingEngine.DependencyResolution.IoC.GetMainContainer();

            using (var nested = container.GetNestedContainer())
            {
                IThemeEngineService themeService = nested.GetInstance<IThemeEngineService>();
                passedString = themeService.GetThemeVariableValue(variableName.Value);
            }

            return new TextNode(passedString);
        }
    }
}