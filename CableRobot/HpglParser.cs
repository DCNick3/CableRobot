using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CableRobot
{
    public class HpglParser
    {
        private Vector2 _pos = new Vector2();

        private HpglParser() { }

        public static IEnumerable<Command> ParseCommands(string str) => new HpglParser().Parse(str);

        private IEnumerable<Command> Parse(string str)
        {
            try
            {
                var res = new List<Command>();
                var _lines = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in Enumerable.Range(0, _lines.Length).Select(i => new { i, v = _lines[i].Trim() }))
                {
                    try
                    {
                        var x = ParseCommand(line.v);
                        foreach (var p in x)
                            p.SourceLine = line.i;
                        res.AddRange(x);
                    }
                    catch (Exception e)
                    {
                        throw new ParseError($"Exception of type {e.GetType().Name} encountered at line {line.i + 1}: {e.Message}");
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

        private IEnumerable<Command> ParseCommand(string commandString)
        {
            string[] parts = commandString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToArray();

            if (parts.Length == 0)
                yield break;

            string commandName = parts[0].Substring(0, 2);
            parts[0] = parts[0].Substring(2);

            switch (commandName)
            {
                case "IN":
                case "SP":
                    break; //Ignore
                case "PU":
                case "PD":
                    {
                        var points = new List<Vector2>();
                        for (int i = 0; i < parts.Length; i += 2)
                            points.Add(new Vector2(int.Parse(parts[i]), int.Parse(parts[i + 1])));
                        if (commandName == "PU")
                            _pos = points.Last();
                        else
                            for (int i = 0; i < points.Count; i++)
                            {
                                yield return new Command(CommandType.Line, new[] { _pos.X, _pos.Y, points[i].X, points[i].Y });
                                _pos = points[i];
                            }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
