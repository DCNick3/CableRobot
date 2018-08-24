using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public class CoordinateComputer
    {
        private const double PenUpZ = 0.0;
        private const double PenDownZ = -55.0;
        // speed is mm/dt
        private const double LinearSpeed = 1.0;
        private const double ZSpeed = 0.25;

        private const double MarginError = 0.5;

        private Vector3 _pos;
        private List<Block<Vector3>> _blocks = new List<Block<Vector3>>();
        private List<Vector3> _points = new List<Vector3>();

        private CoordinateComputer()
        {
            _pos = new Vector3(0, 0, PenUpZ);
        }

        private double X => _pos.X;
        private double Y => _pos.Y;
        private double Z => _pos.Z;
        private Vector2 XYPos => new Vector2(X, Y);

        public static IEnumerable<Block<Vector3>> ComputePoints(IEnumerable<Command> commands) => new CoordinateComputer().ComputeCommands(commands);

        public IEnumerable<Block<Vector3>> ComputeCommands(IEnumerable<Command> commands)
        {
            _points.Clear();
            _blocks.Clear();
            _pos = new Vector3(0, 0, PenUpZ);
            foreach (var cmd in commands)
            {
                switch (cmd.CommandType)
                {
                    case CommandType.Line:
                        GenerateLine(new Vector2(cmd.Parameters[0], cmd.Parameters[1]), new Vector2(cmd.Parameters[2], cmd.Parameters[3]));
                        break;
                    case CommandType.CircleArc:
                        GenerateCircleArc(new Vector2(cmd.Parameters[0], cmd.Parameters[1]), new Vector2(cmd.Parameters[2], cmd.Parameters[3]), cmd.Parameters[4]);
                        break;
                    case CommandType.Circle:
                        {
                            var x = cmd.Parameters[0];
                            var y = cmd.Parameters[1];
                            var r = cmd.Parameters[2];
                            GenerateCircleArc(new Vector2(x, y), new Vector2(x + r, y), 360.0);
                            break;
                        }
                    case CommandType.QuadraticBezierCurve:
                    {
                        GenerateQuadraticBezierCurve(
                            new Vector2(cmd.Parameters[0], cmd.Parameters[1]), 
                            new Vector2(cmd.Parameters[2], cmd.Parameters[3]), 
                            new Vector2(cmd.Parameters[4], cmd.Parameters[5]));
                        break;
                    }
                    case CommandType.CubicBezierCurve:
                    {
                        GenerateCubicBezierCurve(
                            new Vector2(cmd.Parameters[0], cmd.Parameters[1]), 
                            new Vector2(cmd.Parameters[2], cmd.Parameters[3]), 
                            new Vector2(cmd.Parameters[4], cmd.Parameters[5]),
                            new Vector2(cmd.Parameters[6], cmd.Parameters[7]));
                        break;
                    }
                    default:
                        throw new NotImplementedException();
                }
                foreach (var block in _blocks)
                    yield return block;
                _blocks.Clear();
            }

            // Go home at end of execution
            PenUp();
            GoTo(new Vector2());
            foreach (var block in _blocks)
                yield return block;
        }

        private void FlushBlock(string name)
        {
            _blocks.Add(new Block<Vector3>(name, _points.ToArray()));
            _points.Clear();
        }

        private void PrepareGenerate(Vector2 origin)
        {
            if ((origin - XYPos).Length > MarginError)
            {
                PenUp();
                GoTo(origin);
            }
            PenDown();
        }

        private void GenerateLine(Vector2 p1, Vector2 p2)
        {
            PrepareGenerate(p1);
            GoTo(p2);
        }

        private void GenerateCircleArc(Vector2 center, Vector2 start, double angle)
        {
            PrepareGenerate(start);
            GoCircleArc(center, angle);
        }

        private void GenerateQuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            PrepareGenerate(p0);
            GoQuadraticBezierCurve(p0, p1, p2);
        }

        private void GenerateCubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            PrepareGenerate(p0);
            GoCubicBezierCurve(p0, p1, p2, p3);
        }

        private void YieldPos(Vector3 p)
        {
            var dp = p - _pos;
            // Enforce speed limit
            // If something is triggering this exception - it's a bug
            if (dp.Length > LinearSpeed * 2)
                throw new Exception();
            _pos = p;
            _points.Add(p);
        }

        private void YieldPos(double x, double y, double z) => YieldPos(new Vector3(x, y, z));
        private void YieldPos(Vector2 xy) => YieldPos(new Vector3(xy.X, xy.Y, Z));
        private void YieldDelay(int ticks)
        {
            if (ticks != 0)
            {
                for (int i = 0; i < ticks; i++)
                    YieldPos(_pos);
                FlushBlock($"Delay {ticks} ticks");
            }
        }

        private void PenUp()
        {
            for (double z = _pos.Z; z < PenUpZ; z += ZSpeed)
                YieldPos(X, Y, Math.Min(z, PenUpZ));
            // Reduce cumulative 'double' error
            YieldPos(X, Y, PenUpZ);
            FlushBlock("Pen Up");
        }

        private void PenDown()
        {
            for (double z = _pos.Z; z > PenDownZ; z -= ZSpeed)
                YieldPos(X, Y, Math.Max(z, PenDownZ));
            // Reduce cumulative 'double' error
            YieldPos(X, Y, PenDownZ);
            FlushBlock("Pen Down");
        }

        private void GoTo(Vector2 v)
        {
            var diff = (v - XYPos);
            var length = diff.Length;

            var t = (int)Math.Round((v - XYPos).Length / LinearSpeed) + 2;
            Vector2 dpos = diff / t;
            for (int i = 1; i < t; i++)
                YieldPos(XYPos + dpos);
            // Reduce cumulative 'double' error
            YieldPos(v);
            FlushBlock($"GoTo {v}");
        }

        private void GoCircleArc(Vector2 center, double angle)
        {
            angle = angle / 180.0 * Math.PI;
            var r = (center - XYPos).Length;
            var l = Math.Abs(r * angle);

            var t = (int)Math.Round(l / LinearSpeed);

            var da = angle / t;

            var end = (XYPos - center).Rotate(angle) + center;

            var sp = XYPos;

            double a = 0.0;
            for (int i = 1; i < t; i++)
            {
                a += da;
                YieldPos((sp - center).Rotate(a) + center);
            }
            // Reduce cumulative 'double' error
            YieldPos(end);
            FlushBlock($"GoCircleArc {center} {angle}");
        }

        // The method was proudly stolen from https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
        private void GoQuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Vector2 B(double t) =>  p0 * (1 - t) * (1 - t) + p1 * 2 * (1 - t) * t + p2 * t * t;

            double ct = 0.0;

            var v1 = p0 * 2.0 - p1 * 4.0 + p2 * 2.0;
            var v2 = p0 * -2.0 + p1 * 2.0;

            while ((XYPos - p2).Length > LinearSpeed)
            {
                ct += LinearSpeed / (v1 * ct + v2).Length;
                YieldPos(B(ct));
            }
            YieldPos(p2);
            FlushBlock($"GoQuadraticBezierCurve {p0} {p1} {p2}");
        }
        
        private void GoCubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 B(double t) => p0 * (1 - t) * (1 - t) * (1 - t) + 
                                   p1 * 3 * t * (1 - t) * (1 - t) +
                                   p2 * 3 * t * t * (1 - t) + 
                                   p3 * t * t * t; 

            double ct = 0.0;

            var v1 = p0 * -3.0 + p1 * 9.0 + p2 * -9.0 + p3 * 3.0;
            var v2 = p0 * 6.0 + p1 * -12.0 + p2 * 6.0;
            var v3 = p0 * -3.0 + p1 * 3.0;

            while ((XYPos - p3).Length > LinearSpeed)
            {
                var dd = LinearSpeed;
            again:
                var dt = dd / (v1 * ct * ct + v2 * ct + v3).Length;
                var b = B(ct + dt);
                if ((b - XYPos).Length > LinearSpeed * 1.2)
                {
                    // Looks like this method fails sometimes.
                    // Reduce wanted speed and try again!
                    dd /= 2.0;
                    goto again;
                }
                ct += dt;
                YieldPos(B(ct));
            }
            YieldPos(p3);
            FlushBlock($"GoCubicBezierCurve {p0} {p1} {p2} {p3}");
        }
    }
}
