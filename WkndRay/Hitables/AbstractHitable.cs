// -----------------------------------------------------------------------
// <copyright file="AbstractHitable.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Hitables
{
    public abstract class AbstractHitable : IHitable
    {
        public abstract HitRecord Hit(Ray ray, float tMin, float tMax);

        public abstract AABB GetBoundingBox(float t0, float t1);

        public virtual float GetPdfValue(PosVector origin, PosVector v)
        {
            return 1.0f;
        }

        public virtual PosVector Random(PosVector origin)
        {
            return PosVector.UnitX;
        }
    }
}