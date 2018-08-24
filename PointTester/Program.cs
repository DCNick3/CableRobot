using System;
using System.Linq;
using CableRobot;

namespace PointTester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var blocks = new Block<Vector3>[]
            {
                new Block<Vector3>("", 
                    new []
                    {
                        new Vector3(0,      0,     10), 
                        new Vector3(0,      0,     200), 
                        new Vector3(0,      0,     400), 
                        new Vector3(0,      0,     600), 
                        new Vector3(0,      0,     800), 
                        new Vector3(0,      0,     1000),
                    }), 
            };

            var angles = InverseKinematicsComputer.ComputeAngles(blocks).ToArray().Single();

            foreach (var anglese in angles.Elements)
            {
                Console.WriteLine(string.Join(" ", anglese.Thetas.Select(a => a / 180.0 * Math.PI)));
            }
        }
    }
}