// -----------------------------------------------------------------------
// <copyright file="ManySpheresScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Scenes
{
  public class ManySpheresScene : IScene
  {
    public Camera GetCamera(int imageWidth, int imageHeight)
    {
      double aperture = 0.01;
      var lookFrom = new PosVector(24.0, 2.0, 6.0);
      var lookAt = PosVector.UnitY;
      double distanceToFocus = (lookFrom - lookAt).Magnitude();
      return new Camera(
        lookFrom,
        lookAt,
        PosVector.UnitY,
        15.0,
        Convert.ToDouble(imageWidth) / Convert.ToDouble(imageHeight),
        aperture,
        distanceToFocus);
    }

    public IHitable GetWorld()
    {
      var list = new HitableList();

      var checkerTexture = new CheckerTexture(
        new ColorTexture(0.2, 0.3, 0.1),
        new ColorTexture(0.9, 0.9, 0.9),
        PosVector.One * 10.0);

      // original color of large sphere...
      // var colorTexture = new ColorTexture(0.5, 0.5, 0.5);

      list.Add(new Sphere(new PosVector(0.0, -1000.0, 0.0), 1000.0, new LambertianMaterial(checkerTexture)));
      for (int a = -11; a < 11; a++)
      {
        for (int b = -11; b < 11; b++)
        {
          double chooseMat = RandomService.NextDouble();
          var center = new PosVector(
            Convert.ToDouble(a) * RandomService.NextDouble(),
            0.2,
            Convert.ToDouble(b) + 0.9 * RandomService.NextDouble());

          if ((center - new PosVector(4.0, 0.2, 0.0)).Magnitude() > 0.9)
          {
            if (chooseMat < 0.8)
            {
              // diffuse
              list.Add(
                new Sphere(
                  center,
                  0.2,
                  new LambertianMaterial(
                    new ColorTexture(
                      RandomService.NextDouble() * RandomService.NextDouble(),
                      RandomService.NextDouble() * RandomService.NextDouble(),
                      RandomService.NextDouble() * RandomService.NextDouble()))));
            }
            else if (chooseMat < 0.95)
            {
              // metal
              list.Add(
                new Sphere(
                  center,
                  0.2,
                  new MetalMaterial(
                    new ColorVector(
                      0.5 * (1.0 + RandomService.NextDouble()),
                      0.5 * (1.0 + RandomService.NextDouble()),
                      0.5 * (1.0 + RandomService.NextDouble())),
                    0.5 * RandomService.NextDouble())));
            }
            else
            {
              // glass
              list.Add(new Sphere(center, 0.2, new DialectricMaterial(1.5)));
            }
          }
        }
      }

      list.Add(new Sphere(new PosVector(0.0, 1.0, 0.0), 1.0, new DialectricMaterial(1.5)));
      list.Add(new Sphere(new PosVector(-4.0, 1.0, 0.0), 1.0, new LambertianMaterial(new ColorTexture(0.4, 0.2, 0.1))));
      list.Add(new Sphere(new PosVector(4.0, 1.0, 0.0), 1.0, new MetalMaterial(new ColorVector(0.7, 0.6, 0.5), 0.0)));

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