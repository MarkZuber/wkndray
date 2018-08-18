// -----------------------------------------------------------------------
// <copyright file="ManySpheresScene.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay.Scenes
{
  public class ManySpheresScene : IScene
  {
    private readonly IRandomService _randomService;
    public ManySpheresScene(IRandomService randomService)
    {
      _randomService = randomService;
    }

    public Camera GetCamera(int imageWidth, int imageHeight)
    {
      double aperture = 0.01;
      var lookFrom = new PosVector(24.0, 2.0, 6.0);
      var lookAt = PosVector.UnitY;
      double distanceToFocus = (lookFrom - lookAt).Magnitude();
      return new Camera(
        _randomService,
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
      var random = new Random();
      var list = new HitableList();
      list.Add(
        new Sphere(new PosVector(0.0, -1000.0, 0.0), 1000.0, new LambertianMaterial(_randomService, new ColorVector(0.5, 0.5, 0.5))));
      for (int a = -11; a < 11; a++)
      {
        for (int b = -11; b < 11; b++)
        {
          double chooseMat = _randomService.NextDouble();
          var center = new PosVector(
            Convert.ToDouble(a) * _randomService.NextDouble(),
            0.2,
            Convert.ToDouble(b) + 0.9 * _randomService.NextDouble());

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
                    _randomService,
                    new ColorVector(
                      _randomService.NextDouble() * _randomService.NextDouble(),
                      _randomService.NextDouble() * _randomService.NextDouble(),
                      _randomService.NextDouble() * _randomService.NextDouble()))));
            }
            else if (chooseMat < 0.95)
            {
              // metal
              list.Add(
                new Sphere(
                  center,
                  0.2,
                  new MetalMaterial(
                    _randomService,
                    new ColorVector(
                      0.5 * (1.0 + _randomService.NextDouble()),
                      0.5 * (1.0 + _randomService.NextDouble()),
                      0.5 * (1.0 + _randomService.NextDouble())),
                    0.5 * _randomService.NextDouble())));
            }
            else
            {
              // glass
              list.Add(new Sphere(center, 0.2, new DialectricMaterial(_randomService, 1.5)));
            }
          }
        }
      }

      list.Add(new Sphere(new PosVector(0.0, 1.0, 0.0), 1.0, new DialectricMaterial(_randomService, 1.5)));
      list.Add(new Sphere(new PosVector(-4.0, 1.0, 0.0), 1.0, new LambertianMaterial(_randomService, new ColorVector(0.4, 0.2, 0.1))));
      list.Add(new Sphere(new PosVector(4.0, 1.0, 0.0), 1.0, new MetalMaterial(_randomService, new ColorVector(0.7, 0.6, 0.5), 0.0)));

      return list;
    }
  }
}