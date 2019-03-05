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

        private double DMin(double a, double b)
        {
            return a < b ? a : b;
        }

        private double DMax(double a, double b)
        {
            return a > b ? a : b;
        }

        public bool Hit(Ray ray, double tMin, double tMax)
        {
            var minvec = Min.ToDoubleArray();
            var maxvec = Max.ToDoubleArray();
            var originvec = ray.Origin.ToDoubleArray();
            var dirvec = ray.Direction.ToDoubleArray();

            // alternative, slower implementation;
            //for (int a = 0; a < 3; a++)
            //{
            //  double t0 = DMin((minvec[a] - originvec[a]) / dirvec[a], (maxvec[a] - originvec[a]) / dirvec[a]);
            //  double t1 = DMax((minvec[a] - originvec[a]) / dirvec[a], (maxvec[a] - originvec[a]) / dirvec[a]);
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
                double invD = 1.0 / dirvec[a];
                double t0 = (minvec[a] - originvec[a]) * invD;
                double t1 = (maxvec[a] - originvec[a]) * invD;
                if (invD < 0.0)
                {
                    double temp = t0;
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
              Math.Min(Min.X, other.Min.X),
              Math.Min(Min.Y, other.Min.Y),
              Math.Min(Min.Z, other.Min.Z));
            var big = new PosVector(Math.Max(Max.X, other.Max.X), Math.Max(Max.Y, other.Max.Y), Math.Max(Max.Z, other.Max.Z));
            return new AABB(small, big);
        }
    }
}