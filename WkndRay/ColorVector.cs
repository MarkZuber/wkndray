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
      : this(0.0, 0.0, 0.0)
    {
    }

    /// <inheritdoc />
    public ColorVector(double r, double g, double b)
    {
      R = r;
      G = g;
      B = b;
    }

    public double R { get; }
    public double G { get; }
    public double B { get; }

    public static ColorVector Zero => new ColorVector(0.0, 0.0, 0.0);
    public static ColorVector One => new ColorVector(1.0, 1.0, 1.0);

    public ColorVector ClampColor()
    {
      return Clamp(ColorVector.Zero, ColorVector.One);
    }

    public Rgba32 ToRgba32()
    {
      var v2 = ClampColor();
      return new Rgba32(ColorToByte(v2.R), ColorToByte(v2.G), ColorToByte(v2.B));
    }

    private static byte ColorToByte(double c)
    {
      return Convert.ToByte(c * 255.0);
    }

    public override string ToString()
    {
      return string.Format($"({R},{G},{B})");
    }

    private static double ClampValue(double val, double min, double max)
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

    public static double CosVectors(PosVector v1, PosVector v2)
    {
      return v1.Dot(v2) / Math.Sqrt(v1.MagnitudeSquared() * v2.MagnitudeSquared());
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

    public static ColorVector operator *(ColorVector a, double scalar)
    {
      return new ColorVector(a.R * scalar, a.G * scalar, a.B * scalar);
    }

    public static ColorVector operator *(double scalar, ColorVector a)
    {
      return new ColorVector(a.R * scalar, a.G * scalar, a.B * scalar);
    }

    public static ColorVector operator /(ColorVector a, double scalar)
    {
      return new ColorVector(a.R / scalar, a.G / scalar, a.B / scalar);
    }

    public ColorVector AddScaled(ColorVector b, double scale)
    {
      return new ColorVector(R + scale * b.R, G + scale * b.G, B + scale * b.B);
    }

    public ColorVector ApplyGamma2()
    {
      return new ColorVector(Math.Sqrt(R), Math.Sqrt(G), Math.Sqrt(B));
    }
  }
}