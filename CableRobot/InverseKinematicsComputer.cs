using System;
using System.Collections.Generic;
using System.Linq;

namespace CableRobot
{
    public class InverseKinematicsComputer
    {
        private InverseKinematicsComputer()
        {
            BlockRadius = 50.0;
            SpiralHeight = 5.0;
            SpiralLength = 298.0;

            // Proudly stolen from documentation (W was measured by some dudes from bad team)
            // W is length of cable at (0, 0, 0)
            Block1 = new Vector4( 4366.35,  -1943.798, 2893.303, 5722.0772);
            Block2 = new Vector4(-4435.848, -1950.001, 2894.382, 5744.9687);
            Block3 = new Vector4(-4433.995,  1948.413, 2896.691, 5753.3642);
            Block4 = new Vector4( 4368.262,  1842.21,  2896.331, 5733.9592);
        }

        public static IEnumerable<Block<Angles>> ComputeAngles(IEnumerable<Block<Vector3>> blocks) => new InverseKinematicsComputer().ComputeAnglesBlocks(blocks);
        public static IEnumerable<Block<Vector4>> ComputeLengths(IEnumerable<Block<Vector3>> blocks) => new InverseKinematicsComputer().ComputeLengthsBlocks(blocks);
        public static IEnumerable<Block<Angles>> ComputeAnglesFromLengths(IEnumerable<Block<Vector4>> blocks) => new InverseKinematicsComputer().ComputeAnglesFromLengthsBlocks(blocks);
        public static Vector4 ComputeZeroLenths() => new InverseKinematicsComputer().ComputeLZeros();
        
        private Vector4 Block1 { get; }
        private Vector4 Block2 { get; }
        private Vector4 Block3 { get; }
        private Vector4 Block4 { get; }

        // Block Radius
        private double BlockRadius { get; }
        // height of one Spiral step
        private double SpiralHeight { get; }
        // Length of one spiral Vitok
        private double SpiralLength { get; }

        
        private Angles ComputeAngles(Vector3 headPosition)
        {
            var r = new Angles
            {
                Theta1 = ComputeThetaScratch(headPosition, Block1.Vector3, Block1.W),
                Theta2 = ComputeThetaScratch(headPosition, Block2.Vector3, Block2.W),
                Theta3 = ComputeThetaScratch(headPosition, Block3.Vector3, Block3.W),
                Theta4 = ComputeThetaScratch(headPosition, Block4.Vector3, Block4.W)
            };

            if (r.Thetas.Any(_ => double.IsNaN(_) || double.IsInfinity(_)))
                throw new Exception();

            return r;
        }
        private Block<Angles> ComputeAnglesBlock(Block<Vector3> block)
        {
            return new Block<Angles>(block.Name, block.Elements.AsParallel().AsOrdered().Select(_ => ComputeAngles(_)).ToArray());
        }
        private IEnumerable<Block<Angles>> ComputeAnglesBlocks(IEnumerable<Block<Vector3>> blocks)
        {
            return blocks.AsParallel().AsOrdered().Select(_ => ComputeAnglesBlock(_));
        }

        private Vector4 ComputeLengths(Vector3 headPosition)
        {
            var r = new Vector4(
                ComputeLScratch(headPosition, Block1.Vector3),
                ComputeLScratch(headPosition, Block2.Vector3),
                ComputeLScratch(headPosition, Block3.Vector3),
                ComputeLScratch(headPosition, Block4.Vector3)
            );
            return r;
        }
        private Block<Vector4> ComputeLengthsBlock(Block<Vector3> block)
        {
            return new Block<Vector4>(block.Name, block.Elements.AsParallel().AsOrdered().Select(_ => ComputeLengths(_)).ToArray());
        }
        private IEnumerable<Block<Vector4>> ComputeLengthsBlocks(IEnumerable<Block<Vector3>> blocks)
        {
            return blocks.AsParallel().AsOrdered().Select(_ => ComputeLengthsBlock(_));
        }

        private Angles ComputeAnglesFromLengths(Vector4 lengths)
        {
            return new Angles
            {
                Theta1 = ComputeThetaFromL(lengths.X, Block1.W),
                Theta2 = ComputeThetaFromL(lengths.Y, Block2.W),
                Theta3 = ComputeThetaFromL(lengths.Z, Block3.W),
                Theta4 = ComputeThetaFromL(lengths.W, Block4.W),
            };
        }
        private Block<Angles> ComputeAnglesFromLengthsBlock(Block<Vector4> block)
        {
            return new Block<Angles>(block.Name, block.Elements.AsParallel().AsOrdered().Select(_ => ComputeAnglesFromLengths(_)).ToArray());
        }
        private IEnumerable<Block<Angles>> ComputeAnglesFromLengthsBlocks(IEnumerable<Block<Vector4>> blocks)
        {
            return blocks.AsParallel().AsOrdered().Select(_ => ComputeAnglesFromLengthsBlock(_));
        }

        private Vector4 ComputeLZeros()
        {
            var headPosition = new Vector3();
            return  new Vector4(
                ComputeLScratch(headPosition, Block1.Vector3),
                ComputeLScratch(headPosition, Block2.Vector3),
                ComputeLScratch(headPosition, Block3.Vector3),
                ComputeLScratch(headPosition, Block4.Vector3)
            );
        }
        
        #region MATH READ AT YOUR OWN RISK

        private double ComputeCosBeta(Vector3 a, Vector3 c) =>
            (a.X - c.X) / (a - c).Vector2.Length;
        private double ComputeSinBeta(Vector3 a, Vector3 c) =>
            (a.Y - c.Y) / (a - c).Vector2.Length;

        private Vector3 ComputeC1(Vector3 c, double cosBeta, double sinBeta) => 
            c + new Vector3(BlockRadius * cosBeta, BlockRadius * sinBeta, 0);

        private double ComputeCosEpsilon(Vector3 a, Vector3 c1) => 
            BlockRadius / (a - c1).Length;

        private double ComputeCosDelta(Vector3 a, Vector3 c1) => 
            (a - c1).Vector2.Length / (a - c1).Length;

        private double ComputeGamma(Vector3 a, Vector3 c1) => 
            Math.Acos(ComputeCosEpsilon(c1, a)) - Math.Acos(ComputeCosDelta(a, c1));

        private Vector3 ComputeB(Vector3 c1, double gamma, double cosBeta, double sinBeta) =>
            c1 + new Vector3(
                Math.Cos(gamma) * cosBeta,
                Math.Cos(gamma) * sinBeta,
                Math.Sin(gamma)) * BlockRadius;

        private double ComputeL(double gamma, Vector3 a, Vector3 b) => 
            BlockRadius * (Math.PI - gamma) + (a - b).Length;

        private double ComputeH(double deltaL) => SpiralHeight * deltaL / SpiralLength;

        private double ComputeTheta(double deltaL, double h) => 360.0 * (deltaL - h) / SpiralLength;

        private double ComputeThetaScratch(Vector3 a, Vector3 c, double l0) 
            => ComputeThetaFromL(ComputeLScratch(a, c), l0);

        private double ComputeThetaFromL(double l, double l0)
        {
            //r + h / 2 PI
            var deltaL = l - l0;
            var h = ComputeH(deltaL);
            return ComputeTheta(deltaL, h);
        }

        private double ComputeLScratch(Vector3 a, Vector3 c)
        {
            var cosBeta = ComputeCosBeta(a, c);
            var sinBeta = ComputeSinBeta(a, c);
            var c1 = ComputeC1(c, cosBeta, sinBeta);
            var gamma = ComputeGamma(a, c1);
            var b = ComputeB(c1, gamma, cosBeta, sinBeta);
            return ComputeL(gamma, a, b);
        }

        #endregion
    }
}
