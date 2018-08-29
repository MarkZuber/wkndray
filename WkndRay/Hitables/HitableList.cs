// -----------------------------------------------------------------------
// <copyright file="HitableList.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace WkndRay
{
  public class HitableList : List<IHitable>,
                             IHitable
  {
    public HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      HitRecord hitRecord = null;

      double closestSoFar = tMax;
      foreach (IHitable item in this)
      {
        var hr = item.Hit(ray, tMin, closestSoFar);
        if (hr == null)
        {
          continue;
        }

        closestSoFar = hr.T;
        hitRecord = hr;
      }

      return hitRecord;
    }

    /// <inheritdoc />
    public AABB GetBoundingBox(double t0, double t1)
    {
      if (Count < 1)
      {
        return null;
      }

      AABB box = this[0].GetBoundingBox(t0, t1);
      if (box == null)
      {
        return null;
      }

      for (int i = 1; i < Count; i++)
      {
        var tempBox = this[i].GetBoundingBox(t0, t1);
        if (tempBox == null)
        {
          return null;
        }

        box = box.GetSurroundingBox(tempBox);
      }

      return box;
    }

    public double GetPdfValue(PosVector origin, PosVector v)
    {
      double weight = 1.0 / Count;
      double sum = 0.0;
      foreach (var hitable in this)
      {
        sum += weight * hitable.GetPdfValue(origin, v);
      }
      return sum;
    }

    public PosVector Random(PosVector origin)
    {
      int index = Convert.ToInt32(Math.Floor(RandomService.NextDouble() * Convert.ToDouble(Count)));
      return this[index].Random(origin);
    }
  }
}