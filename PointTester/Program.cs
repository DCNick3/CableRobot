using System;
using System.Globalization;
using System.Linq;
using CableRobot;

namespace PointTester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Provide me with some points (double, double, double), and I will compute angles for it");
            while (true)
            {
                var parts = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(_ => double.Parse(_, CultureInfo.InvariantCulture)).ToArray();
                var blocks = new Block<Vector3>[]
                {
                new Block<Vector3>("",
                    new []
                    {
                        new Vector3(parts[0], parts[1], parts[2]),
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
}