using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Svg;

namespace CableRobot
{
    public class SvgParser
    {

        private Vector2 position;
        private Vector2 pathStart;
        
        private SvgParser() { }

        public static IEnumerable<Command> ParseCommands(string str) => new SvgParser().Parse(str);

        private IEnumerable<Command> Parse(string str)
        {
            position = new Vector2();
            pathStart = new Vector2();
            try
            {
                var res = new List<Command>();
                var svg = XDocument.Parse(str);
                foreach (var path in svg.Descendants("{http://www.w3.org/2000/svg}path"))
                {
                    var data = path.Attribute("d");
                    if (data != null)
                    {
                        var val = data.Value;
                        res.AddRange(ParsePath(val));
                    }
                }
                return res;
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

        private Vector2 W(Vector2 v)
        {
            return v - new Vector2(4000, 1900);
        }

        private IEnumerable<Command> ParsePath(string val)
        {
            const string separators = @"(?=[A-Za-z])";
            var tokens = Regex.Split(val, separators).Where(t => !string.IsNullOrEmpty(t));

            foreach (var token in tokens)
            {
                var c = SvgCommand.Parse(token);
                foreach (var cmd in ExecuteCommand(c))
                    yield return cmd;
            }
        }

        private IEnumerable<Command> ExecuteCommand(SvgCommand c)
        {
            switch (c.Command)
            {
                case 'M':
                    pathStart = position = new Vector2(c.Arguments[0], c.Arguments[1]);
                    break;
                case 'm':
                    pathStart = position = position + new Vector2(c.Arguments[0], c.Arguments[1]);
                    break;

                case 'L':
                    L:
                {
                    var p = c.Vector(0);
                    yield return new Command(CommandType.Line,
                        new[] {W(position).X, W(position).Y, W(p).X, W(p).Y});
                    position = p;
                    if (c.Eat(2))
                        goto L;
                    break;
                }
                case 'l':
                    l:
                {
                    var p = c.Vector(0) + position;
                    yield return new Command(CommandType.Line,
                        new[] { W(position).X, W(position).Y, W(p).X, W(p).Y });
                        position = p;
                    if (c.Eat(2))
                        if (c.Arguments.Count != 0)
                            goto l;
                    break;
                }

                case 'V':
                    V:
                    {
                        var p = new Vector2(position.X, c.Arguments[0]);
                        yield return new Command(CommandType.Line,
                        new[] { W(position).X, W(position).Y, W(p).X, W(p).Y });
                        position = p;
                        if (c.Eat(1))
                            goto V;
                        break;
                    }
                case 'v':
                    v:
                    {
                        var p = new Vector2(position.X, position.Y + c.Arguments[0]);
                        yield return new Command(CommandType.Line,
                        new[] { W(position).X, W(position).Y, W(p).X, W(p).Y });
                        position = p;
                        if (c.Eat(1))
                            goto v;
                        break;
                    }

                case 'H':
                    H:
                    {
                        var p = new Vector2(c.Arguments[0], position.Y);
                        yield return new Command(CommandType.Line,
                        new[] { W(position).X, W(position).Y, W(p).X, W(p).Y });
                        position = p;
                        if (c.Eat(1))
                            goto H;
                        break;
                    }
                case 'h':
                    h:
                    {
                        var p = new Vector2(position.X + c.Arguments[0], position.Y);
                        yield return new Command(CommandType.Line,
                        new[] { W(position).X, W(position).Y, W(p).X, W(p).Y });
                        position = p;
                        if (c.Eat(1))
                            goto h;
                        break;
                    }

                case 'C':
                    C:
                {
                    Vector2 p1 = c.Vector(0), p2 = c.Vector(2), p3 = c.Vector(4);
                    yield return new Command(CommandType.CubicBezierCurve,
                        new[] { W(position).X, W(position).Y, W(p1).X, W(p1).Y, W(p2).X, W(p2).Y, W(p3).X, W(p3).Y });
                    position = p3;
                    if (c.Eat(6))
                        goto C;
                    break;
                }
                case 'c':
                    c:
                {
                    Vector2 p1 = position + c.Vector(0), p2 = position + c.Vector(2), p3 = position + c.Vector(4);
                    yield return new Command(CommandType.CubicBezierCurve, 
                        new [] { W(position).X, W(position).Y, W(p1).X, W(p1).Y, W(p2).X, W(p2).Y, W(p3).X, W(p3).Y });
                    position = p3;
                    if (c.Eat(6))
                        goto c;
                    break;
                }
                
                case 'Z':
                case 'z':
                    yield return new Command(CommandType.Line, 
                        new [] { W(position).X, W(position).Y, W(pathStart).X, W(pathStart).Y });
                    position = pathStart;
                    break;
                
                default:
                    throw new NotImplementedException();
            }
        }

        private class SvgCommand
        {
            public char Command { get; }
            public List<double> Arguments { get; }

            public SvgCommand(char command, params double[] arguments)
            {
                this.Command = command;
                this.Arguments = arguments.ToList();
            }

            public static SvgCommand Parse(string SVGpathstring)
            {
                var cmd = SVGpathstring.Take(1).Single();
                var remainingargs = SVGpathstring.Substring(1);

                const string argSeparators = @"[\s,]|(?=-)";
                var splitArgs = Regex
                    .Split(remainingargs, argSeparators)
                    .Where(t => !string.IsNullOrEmpty(t));

                var doubleArgs = splitArgs.Select(a => double.Parse(a, CultureInfo.InvariantCulture)).ToArray();
                return new SvgCommand(cmd, doubleArgs);
            }

            public Vector2 Vector(int startIndex) => new Vector2(Arguments[startIndex], Arguments[startIndex + 1]);

            public bool Eat(int count)
            {
                Arguments.RemoveRange(0, count);
                return Arguments.Count != 0;
            }
        }
    }
}