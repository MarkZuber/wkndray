// -----------------------------------------------------------------------
// <copyright file="XyRect.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay.Hitables
{
  public class XyRect : AbstractHitable
  {
    public XyRect(double x0, double x1, double y0, double y1, double k, IMaterial material)
    {
      X0 = x0;
      X1 = x1;
      Y0 = y0;
      Y1 = y1;
      K = k;
      Material = material;
    }

    public double X0 { get; }
    public double X1 { get; }
    public double Y0 { get; }
    public double Y1 { get; }
    public double K { get; }
    public IMaterial Material { get; }

    public override HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      double t = (K - ray.Origin.Z) / ray.Direction.Z;
      if (t < tMin || t > tMax)
      {
        return null;
      }

      double x = ray.Origin.X + (t * ray.Direction.X);
      double y = ray.Origin.Y + (t * ray.Direction.Y);
      if (x < X0 || x > X1 || y < Y0 || y > Y1)
      {
        return null;
      }

      return new HitRecord(
        t,
        ray.GetPointAtParameter(t),
        PosVector.UnitZ,
        new Point2D((x - X0) / (X1 - X0), (y - Y0) / (Y1 - Y0)),
        Material);
    }

    public override AABB GetBoundingBox(double t0, double t1)
    {
      return new AABB(new PosVector(X0, Y0, K - 0.001), new PosVector(X1, Y1, K + 0.0001));
    }
  }
}