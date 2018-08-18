// -----------------------------------------------------------------------
// <copyright file="Ray.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
  public class Ray
  {
    public Ray(PosVector origin, PosVector direction)
    {
      Origin = origin;
      Direction = direction;
    }

    public PosVector Origin { get; }
    public PosVector Direction { get; }

    public PosVector GetPointAtParameter(double t)
    {
      return Origin + (t * Direction);
    }
  }
}