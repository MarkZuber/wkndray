// -----------------------------------------------------------------------
// <copyright file="ConstantMedium.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Hitables
{
  public class ConstantMedium : AbstractHitable
  {
    public ConstantMedium(IHitable boundary, double density, ITexture a)
    {
      Boundary = boundary;
      Density = density;
      PhaseFunction = new IsotropicMaterial(a);
    }

    public IHitable Boundary { get; }
    public double Density { get; }
    public IMaterial PhaseFunction { get; }

    public override HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      HitRecord hitRecord1 = Boundary.Hit(ray, -double.MaxValue, double.MaxValue);
      if (hitRecord1 == null)
      {
        return null;
      }

      HitRecord hitRecord2 = Boundary.Hit(ray, hitRecord1.T + 0.0001, double.MaxValue);
      if (hitRecord2 == null)
      {
        return null;
      }

      double rec1T = hitRecord1.T;
      double rec2T = hitRecord2.T;

      if (rec1T < tMin)
      {
        rec1T = tMin;
      }

      if (rec2T > tMax)
      {
        rec2T = tMax;
      }

      if (rec1T >= rec2T)
      {
        return null;
      }

      if (rec1T < 0.0)
      {
        rec1T = 0.0;
      }

      double distanceInsideBoundary = ((rec2T - rec1T) * ray.Direction).Magnitude();
      double hitDistance = -(1.0 / Density) * Math.Log(RandomService.NextDouble());
      if (hitDistance < distanceInsideBoundary)
      {
        double recT = rec1T + hitDistance / ray.Direction.Magnitude();

        return new HitRecord(
          recT,
          ray.GetPointAtParameter(recT),
          PosVector.UnitX,  // arbitrary
          new Point2D(0.0, 0.0), // don't need u/v since PhaseFunction is a calculation
          PhaseFunction);
      }

      return null;
    }

    public override AABB GetBoundingBox(double t0, double t1)
    {
      return Boundary.GetBoundingBox(t0, t1);
    }
  }
}