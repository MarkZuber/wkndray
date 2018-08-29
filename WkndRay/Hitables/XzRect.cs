// -----------------------------------------------------------------------
// <copyright file="XyRect.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay.Hitables
{
  public class XzRect : AbstractHitable
  {
    public XzRect(double x0, double x1, double z0, double z1, double k, IMaterial material)
    {
      X0 = x0;
      X1 = x1;
      Z0 = z0;
      Z1 = z1;
      K = k;
      Material = material;
    }

    public double X0 { get; }
    public double X1 { get; }
    public double Z0 { get; }
    public double Z1 { get; }
    public double K { get; }
    public IMaterial Material { get; }

    public override HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      double t = (K - ray.Origin.Y) / ray.Direction.Y;
      if (t < tMin || t > tMax)
      {
        return null;
      }

      double x = ray.Origin.X + t * ray.Direction.X;
      double z = ray.Origin.Z + t * ray.Direction.Z;
      if (x < X0 || x > X1 || z < Z0 || z > Z1)
      {
        return null;
      }

      return new HitRecord(
        t,
        ray.GetPointAtParameter(t),
        PosVector.UnitY,
        new Point2D((x - X0) / (X1 - X0), (z - Z0) / (Z1 - Z0)),
        Material);
    }

    public override AABB GetBoundingBox(double t0, double t1)
    {
      return new AABB(new PosVector(X0, K - 0.001, Z0), new PosVector(X1, K + 0.0001, Z1));
    }

    public override double GetPdfValue(PosVector origin, PosVector v)
    {
      HitRecord hr = Hit(new Ray(origin, v), 0.001, double.MaxValue);
      if (hr == null)
      {
        return 0.0;
      }

      double area = (X1 - X0) * (Z1 - Z0);
      double distanceSquared = hr.T * hr.T * v.MagnitudeSquared();
      double cosine = Math.Abs(v.Dot(hr.Normal) / v.Magnitude());
      return distanceSquared / (cosine * area);
    }

    public override PosVector Random(PosVector origin)
    {
      var randomPoint = new PosVector(X0 + RandomService.NextDouble() * (X1-X0), K, Z0 + RandomService.NextDouble() * (Z1-Z0));
      return randomPoint - origin;
    }
  }
}