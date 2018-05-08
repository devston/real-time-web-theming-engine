using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;

namespace RealTimeThemingEngine.Web.ThemeEngine
{
    public class ContrastFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this, Location);
            Guard.ExpectMaxArguments(4, Arguments.Count, this, Location);
            Guard.ExpectNode<Color>(Arguments[0], this, Location);

            var colour = (Color)Arguments[0];

            if (Arguments.Count > 1)
                Guard.ExpectNode<Color>(Arguments[1], this, Location);
            if (Arguments.Count > 2)
                Guard.ExpectNode<Color>(Arguments[2], this, Location);
            if (Arguments.Count > 3)
                Guard.ExpectNode<Number>(Arguments[3], this, Location);

            var lightColour = Arguments.Count > 1 ? (Color)Arguments[1] : new Color(255d, 255d, 255d);
            var darkColour = Arguments.Count > 2 ? (Color)Arguments[2] : new Color(0d, 0d, 0d);
            var threshold = Arguments.Count > 3 ? ((Number)Arguments[3]).ToNumber() : 0.43d;

            if (darkColour.Luma > lightColour.Luma)
            {
                var tempColour = lightColour;
                lightColour = darkColour;
                darkColour = tempColour;
            }

            return (colour.Luma < threshold) ? lightColour : darkColour;
        }
    }
}