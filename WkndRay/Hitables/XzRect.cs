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
        public XzRect(float x0, float x1, float z0, float z1, float k, IMaterial material)
        {
            X0 = x0;
            X1 = x1;
            Z0 = z0;
            Z1 = z1;
            K = k;
            Material = material;
        }

        public float X0 { get; }
        public float X1 { get; }
        public float Z0 { get; }
        public float Z1 { get; }
        public float K { get; }
        public IMaterial Material { get; }

        public override HitRecord Hit(Ray ray, float tMin, float tMax)
        {
            float t = (K - ray.Origin.Y) / ray.Direction.Y;
            if (t < tMin || t > tMax)
            {
                return null;
            }

            float x = ray.Origin.X + (t * ray.Direction.X);
            float z = ray.Origin.Z + (t * ray.Direction.Z);
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

        public override AABB GetBoundingBox(float t0, float t1)
        {
            return new AABB(new PosVector(X0, K - 0.001f, Z0), new PosVector(X1, K + 0.0001f, Z1));
        }

        public override float GetPdfValue(PosVector origin, PosVector v)
        {
            HitRecord hr = Hit(new Ray(origin, v), 0.001f, float.MaxValue);
            if (hr == null)
            {
                return 0.0f;
            }

            float area = (X1 - X0) * (Z1 - Z0);
            float distanceSquared = hr.T * hr.T * v.MagnitudeSquared();
            float cosine = MathF.Abs(v.Dot(hr.Normal) / v.Magnitude());
            return distanceSquared / (cosine * area);
        }

        public override PosVector Random(PosVector origin)
        {
            var randomPoint = new PosVector(X0 + (RandomService.Nextfloat() * (X1 - X0)), K, Z0 + (RandomService.Nextfloat() * (Z1 - Z0)));
            return randomPoint - origin;
        }
    }
}
