// -----------------------------------------------------------------------
// <copyright file="AbstractMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public abstract class AbstractMaterial : IMaterial
  {
    public abstract ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);

    public ColorVector Emitted(Point2D uvCoords, PosVector p)
    {
      return ColorVector.Zero;
    }
  }
}