// -----------------------------------------------------------------------
// <copyright file="BasicCamera.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

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

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeftCorner + (u * Horizontal) + (v * Vertical) - Origin);
        }
    }
}