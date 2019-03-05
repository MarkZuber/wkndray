// -----------------------------------------------------------------------
// <copyright file="Camera.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
  public class Camera
  {
    /// <summary>
    /// </summary>
    /// <param name="lookFrom"></param>
    /// <param name="lookAt"></param>
    /// <param name="up"></param>
    /// <param name="verticalFov">Top to bottom in degrees</param>
    /// <param name="aspect"></param>
    public Camera(
      PosVector lookFrom,
      PosVector lookAt,
      PosVector up,
      double verticalFov,
      double aspect,
      double aperture,
      double focusDistance)
    {
      LensRadius = aperture / 2.0;
      double theta = verticalFov * Math.PI / 180.0;
      double halfHeight = Math.Tan(theta / 2.0);
      double halfWidth = aspect * halfHeight;
      Origin = lookFrom;
      W = (lookFrom - lookAt).ToUnitVector();
      U = up.Cross(W).ToUnitVector();
      V = W.Cross(U);
      LowerLeftCorner = Origin - (halfWidth * focusDistance * U) - (halfHeight * focusDistance * V) - (focusDistance * W);
      Horizontal = 2.0 * halfWidth * focusDistance * U;
      Vertical = 2.0 * halfHeight * focusDistance * V;
    }

    public PosVector Origin { get; }
    public PosVector LowerLeftCorner { get; }
    public PosVector Horizontal { get; }
    public PosVector Vertical { get; }
    public PosVector U { get; }
    public PosVector V { get; }
    public PosVector W { get; }
    public double LensRadius { get; }

    public Ray GetRay(double s, double t)
    {
      var rd = LensRadius * PosVector.GetRandomInUnitSphere();
      var offset = (U * rd.X) + (V * rd.Y);
      return new Ray(Origin + offset, LowerLeftCorner + (s * Horizontal) + (t * Vertical) - Origin - offset);
    }
  }
}