// -----------------------------------------------------------------------
// <copyright file="IHitable.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
    public interface IHitable
    {
        HitRecord Hit(Ray ray, float tMin, float tMax);
        AABB GetBoundingBox(float t0, float t1);

        float GetPdfValue(PosVector origin, PosVector v);
        PosVector Random(PosVector origin);
    }
}