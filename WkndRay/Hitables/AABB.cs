// -----------------------------------------------------------------------
// <copyright file="AABB.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
    public class AABB
    {
        public AABB(PosVector min, PosVector max)
        {
            Min = min;
            Max = max;
        }

        public PosVector Min { get; }
        public PosVector Max { get; }

        private float DMin(float a, float b)
        {
            return a < b ? a : b;
        }

        private float DMax(float a, float b)
        {
            return a > b ? a : b;
        }

        public bool Hit(Ray ray, float tMin, float tMax)
        {
            var minvec = Min.ToSingleArray();
            var maxvec = Max.ToSingleArray();
            var originvec = ray.Origin.ToSingleArray();
            var dirvec = ray.Direction.ToSingleArray();

            // alternative, slower implementation;
            //for (int a = 0; a < 3; a++)
            //{
            //  float t0 = DMin((minvec[a] - originvec[a]) / dirvec[a], (maxvec[a] - originvec[a]) / dirvec[a]);
            //  float t1 = DMax((minvec[a] - originvec[a]) / dirvec[a], (maxvec[a] - originvec[a]) / dirvec[a]);
            //  tMin = DMax(t0, tMin);
            //  tMax = DMin(t1, tMax);
            //  if (tMax <= tMin)
            //  {
            //    return false;
            //  }
            //}

            //return true;

            for (int a = 0; a < 3; a++)
            {
                float invD = 1.0f / dirvec[a];
                float t0 = (minvec[a] - originvec[a]) * invD;
                float t1 = (maxvec[a] - originvec[a]) * invD;
                if (invD < 0.0f)
                {
                    float temp = t0;
                    t0 = t1;
                    t1 = temp;
                }

                tMin = t0 > tMin ? t0 : tMin;
                tMax = t1 < tMax ? t1 : tMax;
                if (tMax <= tMin)
                {
                    return false;
                }
            }

            return true;
        }

        public AABB GetSurroundingBox(AABB other)
        {
            var small = new PosVector(
              MathF.Min(Min.X, other.Min.X),
              MathF.Min(Min.Y, other.Min.Y),
              MathF.Min(Min.Z, other.Min.Z));
            var big = new PosVector(MathF.Max(Max.X, other.Max.X), MathF.Max(Max.Y, other.Max.Y), MathF.Max(Max.Z, other.Max.Z));
            return new AABB(small, big);
        }
    }
}