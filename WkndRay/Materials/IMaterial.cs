using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Materials
{
  public interface IMaterial
  {
    ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);
  }
}
