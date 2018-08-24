using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    // Custom Vector was introduced to add double support to reduce floating point number errors
    public struct Vector2
    {
        public double X, Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vector2 Rotate(double theta)
        {
            // https://en.wikipedia.org/wiki/Rotation_matrix
            return new Vector2(
                    X * Math.Cos(theta) - Y * Math.Sin(theta),
                    X * Math.Sin(theta) + Y * Math.Cos(theta)
                );
        }

        public Vector2 Normalize()
        {
            var l = Length;
            if (l == 0.0)
                return new Vector2();
            return this / l;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
            {
                return false;
            }

            var vector = (Vector2)obj;
            return X == vector.X &&
                   Y == vector.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{'{'} {X}, {Y} {'}'}";

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, double f) => new Vector2(a.X * f, a.Y * f);
        public static Vector2 operator /(Vector2 a, double f) => new Vector2(a.X / f, a.Y / f);
        public static bool operator ==(Vector2 a, Vector2 b) => a != null && a.Equals(b);
        public static bool operator !=(Vector2 a, Vector2 b) => a == null || !a.Equals(b);
    }

    public struct Vector3
    {
        public double X, Y, Z;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 v, double z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }


        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
        public Vector2 Vector2 => new Vector2(X, Y);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
            {
                return false;
            }

            var vector = (Vector3)obj;
            return X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{'{'} {X}, {Y}, {Z} {'}'}";

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 a, double f) => new Vector3(a.X * f, a.Y * f, a.Z * f);
        public static Vector3 operator /(Vector3 a, double f) => new Vector3(a.X / f, a.Y / f, a.Z * f);
        public static bool operator ==(Vector3 a, Vector3 b) => a != null && a.Equals(b);
        public static bool operator !=(Vector3 a, Vector3 b) => a == null || !a.Equals(b);
    }

    public struct Vector4
    {
        public double X, Y, Z, W;

        public Vector4(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        public Vector2 Vector2 => new Vector2(X, Y);
        public Vector3 Vector3 => new Vector3(X, Y, Z);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector4))
            {
                return false;
            }

            var vector = (Vector4)obj;
            return X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z &&
                   W == vector.W;
        }

        public override int GetHashCode()
        {
            var hashCode = 707706286;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{'{'} {X}, {Y}, {Z}, {W} {'}'}";

        public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Vector4 operator *(Vector4 a, double f) => new Vector4(a.X * f, a.Y * f, a.Z * f, a.W * f);
        public static Vector4 operator /(Vector4 a, double f) => new Vector4(a.X / f, a.Y / f, a.Z * f, a.W / f);
        public static bool operator ==(Vector4 a, Vector4 b) => a != null && a.Equals(b);
        public static bool operator !=(Vector4 a, Vector4 b) => a == null || !a.Equals(b);
    }
}
