using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CableRobot
{
    public class Command
    {
        private static readonly Dictionary<CommandType, string> CommandNames = new Dictionary<CommandType, string>()
        {
            { CommandType.Line, "line" },
            { CommandType.CircleArc, "circle_arc" },
            { CommandType.Circle, "circle"},
            { CommandType.QuadraticBezierCurve, "quadratic_bezier_curve" },
            { CommandType.CubicBezierCurve, "cubic_bezier_curve" },
        };

        public Command(CommandType commandType, double[] parameters)
        {
            CommandType = commandType;
            Parameters = parameters;
        }

        public CommandType CommandType { get; set; }
        public double[] Parameters { get; set; }
        public int SourceLine { get; set; }

        public override string ToString() =>
            $"{CommandNames[CommandType]} {string.Join(" ", Parameters.Select(_ => _.ToString(CultureInfo.InvariantCulture)))}";
    }
}
