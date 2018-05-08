using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;
using System.Linq;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    public class CalculateRemFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count(), this, Location);
            Guard.ExpectNode<Number>(Arguments[0], this, Arguments[0].Location);
            var size = Arguments[0] as Number;
            double value = size.Value;

            // Divide the value by the base font size (16) to get the rem value.
            double result = value / 16;
            return new Number(result, "rem");
        }
    }
}