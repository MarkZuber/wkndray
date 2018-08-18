using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WkndRay
{
  public class BasicCamera
  {
    public BasicCamera(PosVector origin, PosVector lowerLeftCorner, PosVector horizontal, PosVector vertical)
    {
      Origin = origin;
      LowerLeftCorner = lowerLeftCorner;
      Horizontal = horizontal;
      Vertical = vertical;
    }

    public PosVector Origin { get; }
    public PosVector LowerLeftCorner { get; }
    public PosVector Horizontal { get; }
    public PosVector Vertical { get; }

    public Ray GetRay(double u, double v)
    {
      return new Ray(Origin, LowerLeftCorner + u*Horizontal + v * Vertical - Origin);
    }
  }
}
