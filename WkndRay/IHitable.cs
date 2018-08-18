using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public interface IHitable
  {
    HitRecord Hit(Ray ray, double tMin, double tMax);
  }
}
