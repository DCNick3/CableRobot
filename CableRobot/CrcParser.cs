using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CableRobot
{
    /// <summary>
    /// Parses CableRobotCode - intermediate representation of trajectory with geometric primitives
    /// </summary>
    public class CrcParser
    {
        private readonly Dictionary<string, CommandType> _commandTypes = new Dictionary<string, CommandType>()
        {
            { "line", CommandType.Line },
            { "circle_arc", CommandType.CircleArc },
            { "circle", CommandType.Circle},
            { "quadratic_bezier_curve", CommandType.QuadraticBezierCurve },
            { "cubic_bezier_curve", CommandType.CubicBezierCurve },
        };

        private readonly Dictionary<CommandType, int> _commandArgumentLengths = new Dictionary<CommandType, int>()
        {
            { CommandType.Line, 4 },
            { CommandType.CircleArc, 5 },
            { CommandType.Circle, 3 },
            { CommandType.QuadraticBezierCurve, 6 },
            { CommandType.CubicBezierCurve, 8 },
        };

        private CrcParser() { }

        public static IEnumerable<Command> ParseCommands(string str) => new CrcParser().Parse(str);

        private IEnumerable<Command> Parse(string str)
        {
            try
            {
                var _lines = str.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                return Enumerable.Range(0, _lines.Length)
                    .AsParallel().AsOrdered()
                    .Select(i => new { i, v = _lines[i] })
                    .Select(line =>
                    {
                        try
                        {
                            var x = ParseCommand(line.v);
                            if (x != null)
                                x.SourceLine = line.i;
                            return x;
                        }
                        catch (Exception e)
                        {
                            throw new ParseError($"Exception of type {e.GetType().Name} encountered at line {line.i + 1}: {e.Message}");
                        }
                    })
                    .Where(line => line != null);
            }
            catch (AggregateException e)
            {
                var exceptions = e.InnerExceptions;
                var sb = new StringBuilder();
                foreach (var exception in exceptions)
                    sb.AppendLine(exception.Message);
                throw new ParseError(sb.ToString());
            }
        }

        private Command ParseCommand(string commandString)
        {
            string[] parts = commandString.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return null;

            var commandName = parts[0];

            if (commandName.StartsWith("#"))
                return null;
            
            if (!_commandTypes.TryGetValue(commandName, out var commandType))
                throw new ParseError("Unknown command");

            double[] parameters = parts.Skip(1).Select(_ => double.Parse(_, CultureInfo.InvariantCulture)).ToArray();

            if (_commandArgumentLengths[commandType] != parameters.Length)
                throw new ParseError("Wrong parameter count");

            return new Command(commandType, parameters);
        }
    }
}
