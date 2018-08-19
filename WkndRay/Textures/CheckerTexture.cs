using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Textures
{
  public class CheckerTexture : ITexture
  {
    public CheckerTexture(ITexture t1, ITexture t2, PosVector scale)
    {
      T1 = t1;
      T2 = t2;
      Scale = scale;
    }

    public ITexture T1 { get; }
    public ITexture T2 { get; }
    public PosVector Scale { get; }

    /// <inheritdoc />
    public ColorVector GetValue(double u, double v, PosVector p)
    {
      double sines = Math.Sin(Scale.X * p.X) * Math.Sin(Scale.Y * p.Y) * Math.Sin(Scale.Z * p.Z);
      return (sines < 0.0) ? T1.GetValue(u, v, p) : T2.GetValue(u, v, p);
    }
  }
}
