using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay
{
  public interface IRayTracer
  {
    ColorVector GetPixelColor(int x, int y);
  }
}
