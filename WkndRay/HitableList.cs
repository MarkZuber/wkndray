﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public class HitableList : List<IHitable>, IHitable
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
  }
}