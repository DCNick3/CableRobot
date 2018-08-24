using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace CableRobot
{
    public class SvgGenerator
    {
        private const string StrokeWidthMagic = "$$strokeWidth";

        private List<string> _elements = new List<string>();

        public SvgGenerator()
        {
            StrokeColor = "black";
        }

        public int MinX => -4500;
        public int MaxX => 4500;   
        public int MinY => -2000;
        public int MaxY => 2000;

        public string StrokeColor { get; set; }

        // double.ToString, that does not depend on locale
        private static string D(double d) => d.ToString(CultureInfo.InvariantCulture);

        public string GenerateCode(int strokeWidth)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" standalone=\"no\"?>");
            sb.AppendLine($"<svg width=\"{MaxX - MinX}\" height=\"{MaxY - MinY}\" xmlns=\"http://www.w3.org/2000/svg\">");
            foreach (var element in _elements)
                sb.AppendLine($"    {element}".Replace(StrokeWidthMagic, strokeWidth.ToString()));
            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        public void ExecuteCommand(Command cmd)
        {
            switch (cmd.CommandType)
            {
                case CommandType.Line:
                    DrawLine(new Vector2(cmd.Parameters[0], cmd.Parameters[1]),
                        new Vector2(cmd.Parameters[2], cmd.Parameters[3]));
                    break;
                case CommandType.Circle:
                    DrawCircle(new Vector2(cmd.Parameters[0], cmd.Parameters[1]), cmd.Parameters[2]);
                    break;
                case CommandType.CircleArc:
                    DrawCircleArc(new Vector2(cmd.Parameters[0], cmd.Parameters[1]),
                        new Vector2(cmd.Parameters[2], cmd.Parameters[3]), cmd.Parameters[4]);
                    break;
                case CommandType.QuadraticBezierCurve:
                    DrawQuadraticBezierCurve(
                        new Vector2(cmd.Parameters[0], cmd.Parameters[1]),
                        new Vector2(cmd.Parameters[2], cmd.Parameters[3]),
                        new Vector2(cmd.Parameters[4], cmd.Parameters[5]));
                    break;
                case CommandType.CubicBezierCurve:
                    DrawCubicBezierCurve(
                        new Vector2(cmd.Parameters[0], cmd.Parameters[1]),
                        new Vector2(cmd.Parameters[2], cmd.Parameters[3]),
                        new Vector2(cmd.Parameters[4], cmd.Parameters[5]),
                        new Vector2(cmd.Parameters[6], cmd.Parameters[7]));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void DrawLine(Vector2 p1, Vector2 p2)
        {
            p1 = TransformPoint(p1);
            p2 = TransformPoint(p2);

            _elements.Add($"<line x1=\"{D(p1.X)}\" y1=\"{D(p1.Y)}\" x2=\"{D(p2.X)}\" y2=\"{D(p2.Y)}\" stroke=\"{StrokeColor}\" stroke-width=\"{StrokeWidthMagic}\"/>");
        }

        public void DrawCircle(Vector2 center, double radius)
        {
            center = TransformPoint(center);

            _elements.Add($"<circle cx=\"{D(center.X)}\" cy=\"{D(center.Y)}\" r=\"{D(radius)}\" " +
                $"stroke=\"{StrokeColor}\" stroke-width=\"{StrokeWidthMagic}\" fill=\"transparent\"/>");
        }

        public void DrawCircleArc(Vector2 center, Vector2 start, double angle)
        {
            center = TransformPoint(center);
            start = TransformPoint(start);
            angle = angle / 180.0 * Math.PI;

            double r = (start - center).Length;

            // Compute end point vector
            // Move (0, 0) to center, rotate it, move it back
            var end = (start - center).Rotate(angle) + center;

            PutArc(start.X, start.Y, r, r, 0, Math.Abs(angle) > Math.PI, angle > 0, end.X, end.Y);
        }

        public void DrawQuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            p0 = TransformPoint(p0);
            p1 = TransformPoint(p1);
            p2 = TransformPoint(p2);
            
            _elements.Add($"<path d=\"M {D(p0.X)} {D(p0.Y)} Q {D(p1.X)} {D(p1.Y)} {D(p2.X)} {D(p2.Y)}\"" +
                          $" stroke=\"{StrokeColor}\" stroke-width=\"{StrokeWidthMagic}\" fill=\"transparent\"/>");

        }

        public void DrawCubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            p0 = TransformPoint(p0);
            p1 = TransformPoint(p1);
            p2 = TransformPoint(p2);
            p3 = TransformPoint(p3);
            
            _elements.Add($"<path d=\"M {D(p0.X)} {D(p0.Y)} C {D(p1.X)} {D(p1.Y)} {D(p2.X)} {D(p2.Y)} {D(p3.X)} {D(p3.Y)}\"" +
                          $" stroke=\"{StrokeColor}\" stroke-width=\"{StrokeWidthMagic}\" fill=\"transparent\"/>");

        }


        private void PutArc(double x, double y, double rx, double ry, double rotate, bool largeArc, bool sweepDirection, double endX, double endY)
        {
            _elements.Add($"<path d=\"M {D(x)} {D(y)} A {D(rx)} {D(ry)} {D(rotate)} {(largeArc ? 1 : 0)} {(sweepDirection ? 1 : 0)} {D(endX)} {D(endY)}\"" +
                $" stroke=\"{StrokeColor}\" stroke-width=\"{StrokeWidthMagic}\" fill=\"transparent\"/>");
        }

        private Vector2 TransformPoint(Vector2 point)
        {
            // TODO: Fix Y direction
            if (point.X < MinX || point.X > MaxX || point.Y < MinY || point.Y > MaxY)
                throw new ExecutionError("Point is out of bounds");
            var p = new Vector2(point.X - MinX, MaxY - point.Y);
            return p;
        }
    }
}
