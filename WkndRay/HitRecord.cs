using System;
using System.Collections.Generic;
using System.Text;
using WkndRay.Materials;

namespace WkndRay
{
  public class HitRecord
  {
    public HitRecord(double t, PosVector p, PosVector normal, IMaterial material = null)
    {
      T = t;
      P = p;
      Normal = normal;
      Material = material;
    }

    public double T { get; }
    public PosVector P { get; }
    public PosVector Normal { get; }
    public IMaterial Material { get; }
  }
}
