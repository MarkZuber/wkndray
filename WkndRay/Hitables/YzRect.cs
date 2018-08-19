// -----------------------------------------------------------------------
// <copyright file="XyRect.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay.Hitables
{
  public class YzRect : IHitable
  {
    public YzRect(double y0, double y1, double z0, double z1, double k, IMaterial material)
    {
      Y0 = y0;
      Y1 = y1;
      Z0 = z0;
      Z1 = z1;
      K = k;
      Material = material;
    }

    public double Y0 { get; }
    public double Y1 { get; }
    public double Z0 { get; }
    public double Z1 { get; }
    public double K { get; }
    public IMaterial Material { get; }

    /// <inheritdoc />
    public HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      double t = (K - ray.Origin.X) / ray.Direction.X;
      if (t < tMin || t > tMax)
      {
        return null;
      }

      double y = ray.Origin.Y + t * ray.Direction.Y;
      double z = ray.Origin.Z + t * ray.Direction.Z;
      if (y < Y0 || y > Y1 || z < Z0 || z > Z1)
      {
        return null;
      }

      return new HitRecord(
        t,
        ray.GetPointAtParameter(t),
        PosVector.UnitX,
        new Point2D((y - Y0) / (Y1 - Y0), (z - Z0) / (Z1 - Z0)),
        Material);
    }

    /// <inheritdoc />
    public AABB GetBoundingBox(double t0, double t1)
    {
      return new AABB(new PosVector(K - 0.001, Y0, Z0), new PosVector(K + 0.0001, Y1, Z1));
    }
  }
}