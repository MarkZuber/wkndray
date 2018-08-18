// -----------------------------------------------------------------------
// <copyright file="PosVector.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
  public class PosVector
  {
    public PosVector(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public static PosVector Zero => new PosVector(0.0, 0.0, 0.0);
    public static PosVector One => new PosVector(1.0, 1.0, 1.0);
    public static PosVector UnitX => new PosVector(1.0, 0.0, 0.0);
    public static PosVector UnitY => new PosVector(0.0, 1.0, 0.0);
    public static PosVector UnitZ => new PosVector(0.0, 0.0, 1.0);

    public override string ToString()
    {
      return string.Format($"({X},{Y},{Z})");
    }

    private static double ClampValue(double val, double min, double max)
    {
      if (val < min)
      {
        return min;
      }

      return val > max ? max : val;
    }

    public PosVector Clamp(PosVector min, PosVector max)
    {
      return new PosVector(ClampValue(X, min.X, max.X), ClampValue(Y, min.Y, max.Y), ClampValue(Z, min.Z, max.Z));
    }

    public static double CosVectors(PosVector v1, PosVector v2)
    {
      return v1.Dot(v2) / Math.Sqrt(v1.MagnitudeSquared() * v2.MagnitudeSquared());
    }

    public double MagnitudeSquared()
    {
      return this.Dot(this);
    }

    public double Magnitude()
    {
      return Math.Sqrt(MagnitudeSquared());
    }

    public PosVector Normalize()
    {
      return this / Magnitude();
    }

    public ColorVector ToColorVector()
    {
      return new ColorVector(X, Y, Z);
    }

    public PosVector ToUnitVector()
    {
      double k = 1.0 / Magnitude();
      return new PosVector(X * k, Y * k, Z * k);
    }

    public static PosVector operator +(PosVector a, PosVector b)
    {
      return new PosVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static PosVector operator -(PosVector a, PosVector b)
    {
      return new PosVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static PosVector operator -(PosVector a)
    {
      return new PosVector(-a.X, -a.Y, -a.Z);
    }

    public static PosVector operator *(PosVector a, double scalar)
    {
      return new PosVector(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }

    public static PosVector operator *(double scalar, PosVector a)
    {
      return new PosVector(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }

    public static PosVector operator /(PosVector a, double scalar)
    {
      return new PosVector(a.X / scalar, a.Y / scalar, a.Z / scalar);
    }

    public PosVector AddScaled(PosVector b, double scale)
    {
      return new PosVector(X + scale * b.X, Y + scale * b.Y, Z + scale * b.Z);
    }

    public PosVector Cross(PosVector b)
    {
      return new PosVector(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);
    }

    public double Dot(PosVector b)
    {
      return X * b.X + Y * b.Y + Z * b.Z;
    }

    public static PosVector GetRandomInUnitSphere()
    {
      PosVector pv;
      do
      {
        pv = 2.0 * new PosVector(RandomService.NextDouble(), RandomService.NextDouble(), RandomService.NextDouble()) -
             PosVector.One;
      }
      while (pv.MagnitudeSquared() >= 1.0);

      return pv;
    }

    public PosVector Reflect(PosVector other)
    {
      return this - 2 * Dot(other) * other;
    }

    public PosVector Refract(PosVector normal, double niOverNt)
    {
      var unitVector = ToUnitVector();
      double dt = unitVector.Dot(normal);
      double discriminant = 1.0 - niOverNt * niOverNt * (1.0 - dt * dt);
      if (discriminant > 0.0)
      {
        return niOverNt * (unitVector - normal * dt) - normal * Math.Sqrt(discriminant);
      }

      return null;
    }
  }
}