using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public static class CodeProcessor
    {
        /// <summary>
        /// Constructs code processing conveyor and executes it
        /// </summary>
        public static IEnumerable<Block<Angles>> CodeToAngles(string code) =>
            InverseKinematicsComputer.ComputeAngles(
                CoordinateComputer.ComputePoints(
                    CrcParser.ParseCommands(code)
                )
            );

        public static IEnumerable<Block<Vector4>> CodeToLengths(string code) =>
            InverseKinematicsComputer.ComputeLengths(
                CoordinateComputer.ComputePoints(
                    CrcParser.ParseCommands(code)
                )
            );

        public static string HpglToCrc(string hpgl)
        {
            var commands = HpglParser.ParseCommands(hpgl);
            return string.Join("\n", commands);
        }

        public static string SvgToCrc(string code)
        {
            var commands = SvgParser.ParseCommands(code);
            return string.Join("\n", commands);
        }

        public static IEnumerable<Block<Vector3>> CodeToPoints(string code)
        {
            return CoordinateComputer.ComputePoints(CrcParser.ParseCommands(code));
        }

        /*
        public static string CrcToLines(string code)
        {
            var commands = CrcParser.ParseCommands(code);
            foreach (var c)
        }*/
    }
}
