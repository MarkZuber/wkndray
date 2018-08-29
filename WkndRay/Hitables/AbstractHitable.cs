// -----------------------------------------------------------------------
// <copyright file="AbstractHitable.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Hitables
{
  public abstract class AbstractHitable : IHitable
  {
    public abstract HitRecord Hit(Ray ray, double tMin, double tMax);

    public abstract AABB GetBoundingBox(double t0, double t1);

    public virtual double GetPdfValue(PosVector origin, PosVector v)
    {
      return 1.0;
    }

    public virtual PosVector Random(PosVector origin)
    {
      return PosVector.UnitX;
    }
  }
}