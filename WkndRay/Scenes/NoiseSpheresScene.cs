// -----------------------------------------------------------------------
// <copyright file="NoiseSpheresScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
  public class NoiseSpheresScene : IScene
  {
    /// <inheritdoc />
    public Camera GetCamera(int imageWidth, int imageHeight)
    {
      var lookFrom = new PosVector(13.0, 2.0, 3.0);
      var lookAt = PosVector.Zero;
      double distToFocus = 10.0;
      double aperture = 0.0;
      return new Camera(
        lookFrom,
        lookAt,
        PosVector.UnitY,
        20.0,
        Convert.ToDouble(imageWidth) / Convert.ToDouble(imageHeight),
        aperture,
        distToFocus);
    }

    /// <inheritdoc />
    public IHitable GetWorld()
    {
      //var perlinTexture = new NoiseTexture(true, 3.0);
      var list = new HitableList()
      {
        new Sphere(
          new PosVector(0.0, -1000.0, 0.0),
          1000.0,
          new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Soft, 3.0))),
        new Sphere(
          new PosVector(0.0, 2.0, 0.0),
          2.0,
          new LambertianMaterial(new VectorNoiseTexture(VectorNoiseMode.Marble, 3.0)))
      };

      return new BvhNode(list, 0.0, 1.0);
    }

    /// <inheritdoc />
    public IHitable GetLightHitable()
    {
      return new HitableList();
    }

    /// <inheritdoc />
    public Func<Ray, ColorVector> GetBackgroundFunc()
    {
      return ray =>
      {
        var unitDirection = ray.Direction.ToUnitVector();
        double t = 0.5 * (unitDirection.Y + 1.0);
        return (((1.0 - t) * ColorVector.One) + t * new ColorVector(0.5, 0.7, 1.0));
      };
    }
  }
}