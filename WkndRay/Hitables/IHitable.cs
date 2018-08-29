// -----------------------------------------------------------------------
// <copyright file="IHitable.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
  public interface IHitable
  {
    HitRecord Hit(Ray ray, double tMin, double tMax);
    AABB GetBoundingBox(double t0, double t1);

    double GetPdfValue(PosVector origin, PosVector v);
    PosVector Random(PosVector origin);
  }
}