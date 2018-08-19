using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Textures
{
  public interface ITexture
  {
    ColorVector GetValue(double u, double v, PosVector p);
  }
}
