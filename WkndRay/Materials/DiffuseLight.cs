// -----------------------------------------------------------------------
// <copyright file="DiffuseLight.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using WkndRay.Textures;

namespace WkndRay.Materials
{
  public class DiffuseLight : IMaterial
  {
    public DiffuseLight(ITexture texture)
    {
      Texture = texture;
    }

    public ITexture Texture { get; }

    public ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
    {
      return ScatterResult.False();
    }

    public ColorVector Emitted(Point2D uvCoords, PosVector p)
    {
      return Texture.GetValue(uvCoords, p);
    }
  }
}