// -----------------------------------------------------------------------
// <copyright file="ColorVector.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using SixLabors.ImageSharp.PixelFormats;

namespace WkndRay
{
    public class ColorVector
    {
        public ColorVector()
          : this(0.0f, 0.0f, 0.0f)
        {
        }

        /// <inheritdoc />
        public ColorVector(float r, float g, float b)
        {
            R = float.IsNaN(r) ? 0.0f : r;
            G = float.IsNaN(g) ? 0.0f : g;
            B = float.IsNaN(b) ? 0.0f : b;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }

        public static ColorVector Zero => new ColorVector(0.0f, 0.0f, 0.0f);
        public static ColorVector One => new ColorVector(1.0f, 1.0f, 1.0f);

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
            return Clamp(ColorVector.Zero, ColorVector.One);
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

        private static float ClampValue(float val, float min, float max)
        {
            if (val < min)
            {
                return min;
            }

            return val > max ? max : val;
        }

        public ColorVector Clamp(ColorVector min, ColorVector max)
        {
            return new ColorVector(ClampValue(R, min.R, max.R), ClampValue(G, min.G, max.G), ClampValue(B, min.B, max.B));
        }

        public static float CosVectors(PosVector v1, PosVector v2)
        {
            return v1.Dot(v2) / MathF.Sqrt(v1.MagnitudeSquared() * v2.MagnitudeSquared());
        }

        public PosVector ToPosVector()
        {
            return new PosVector(R, G, B);
        }

        public static ColorVector operator +(ColorVector a, ColorVector b)
        {
            return new ColorVector(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static ColorVector operator -(ColorVector a, ColorVector b)
        {
            return new ColorVector(a.R - b.R, a.G - b.G, a.B - b.B);
        }

        public static ColorVector operator *(ColorVector a, ColorVector b)
        {
            return new ColorVector(a.R * b.R, a.G * b.G, a.B * b.B);
        }

        public static ColorVector operator *(ColorVector a, float scalar)
        {
            return new ColorVector(a.R * scalar, a.G * scalar, a.B * scalar);
        }

        public static ColorVector operator *(float scalar, ColorVector a)
        {
            return new ColorVector(a.R * scalar, a.G * scalar, a.B * scalar);
        }

        public static ColorVector operator /(ColorVector a, float scalar)
        {
            return new ColorVector(a.R / scalar, a.G / scalar, a.B / scalar);
        }

        public ColorVector AddScaled(ColorVector b, float scale)
        {
            return new ColorVector(R + (scale * b.R), G + (scale * b.G), B + (scale * b.B));
        }

        public ColorVector ApplyGamma2()
        {
            return new ColorVector(MathF.Sqrt(R), MathF.Sqrt(G), MathF.Sqrt(B));
        }
    }
}
