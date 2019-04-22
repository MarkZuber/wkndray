// -----------------------------------------------------------------------
// <copyright file="ColorVector.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;

namespace WkndRay
{
    public struct ColorVector 
    {
        private readonly Vector3 _vector3;

        /// <inheritdoc />
        public ColorVector(float r, float g, float b)
        {
            _vector3 = new Vector3(float.IsNaN(r) ? 0.0f : r, float.IsNaN(g) ? 0.0f : g, float.IsNaN(b) ? 0.0f : b);
        }

        private ColorVector(Vector3 vec)
        {
            _vector3 = vec;
        }

        public float R => _vector3.X;
        public float G => _vector3.Y;
        public float B => _vector3.Z;

        public static ColorVector Zero => new ColorVector(Vector3.Zero);
        public static ColorVector One => new ColorVector(Vector3.One);

        public static ColorVector FromBytes(byte r, byte g, byte b)
        {
            return new ColorVector(ByteToColor(r), ByteToColor(g), ByteToColor(b));
        }

        public ColorVector DeNan()
        {
            return new ColorVector(
                float.IsNaN(R) ? 0.0f : R,
                float.IsNaN(G) ? 0.0f : G,
                float.IsNaN(B) ? 0.0f : B);
        }

        public ColorVector ClampColor()
        {
            return new ColorVector(Vector3.Clamp(_vector3, Vector3.Zero, Vector3.One));
        }

        public Rgba32 ToRgba32()
        {
            var v2 = ClampColor();
            return new Rgba32(ColorToByte(v2.R), ColorToByte(v2.G), ColorToByte(v2.B));
        }

        private static byte ColorToByte(float c)
        {
            try
            {
                return Convert.ToByte(c * 255.0f);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static float ByteToColor(byte c)
        {
            return Convert.ToSingle(c) / 255.0f;
        }

        public override string ToString()
        {
            return string.Format($"({R},{G},{B})");
        }

        public static float CosVectors(Vector3 v1, Vector3 v2)
        {
            return Vector3.Dot(v1, v2) / MathF.Sqrt(v1.LengthSquared() * v2.LengthSquared());
        }

        public static ColorVector operator +(ColorVector a, ColorVector b)
        {
            return new ColorVector(a._vector3 + b._vector3);
        }

        public static ColorVector operator -(ColorVector a, ColorVector b)
        {
            return new ColorVector(a._vector3 - b._vector3);
        }

        public static ColorVector operator *(ColorVector a, ColorVector b)
        {
            return new ColorVector(a._vector3 * b._vector3);
        }

        public static ColorVector operator *(ColorVector a, float scalar)
        {
            return new ColorVector(a._vector3 * scalar);
        }

        public static ColorVector operator *(float scalar, ColorVector a)
        {
            return new ColorVector(a._vector3 * scalar);
        }

        public static ColorVector operator /(ColorVector a, float scalar)
        {
            return new ColorVector(a._vector3 / scalar);
        }

        public ColorVector ApplyGamma2()
        {
            return new ColorVector(Vector3.SquareRoot(_vector3));
            // return new ColorVector(MathF.Sqrt(R), MathF.Sqrt(G), MathF.Sqrt(B));
        }
    }
}
