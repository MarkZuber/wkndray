// -----------------------------------------------------------------------
// <copyright file="BetterCameraGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay.Executors
{
  public class BetterCameraGenerator : IExecutor
  {
    private readonly int _numSamples;

    public BetterCameraGenerator(int numSamples)
    {
      _numSamples = numSamples;
    }

    public PixelBuffer Execute(int width, int height)
    {
      var pixelBuffer = new PixelBuffer(width, height);
      double aperture = 2.0;
      var lookFrom = new PosVector(3.0, 3.0, 2.0);
      var lookAt = new PosVector(0.0, 0.0, -1.0);
      double distanceToFocus = (lookFrom - lookAt).Magnitude();
      var camera = new Camera(
        lookFrom,
        lookAt,
        new PosVector(0.0, 1.0, 0.0),
        30.0,
        Convert.ToDouble(width) / Convert.ToDouble(height),
        aperture,
        distanceToFocus);

      var hitables = new HitableList
      {
        new Sphere(new PosVector(0.0, 0.0, -1.0), 0.5, new LambertianMaterial(new ColorVector(0.1, 0.2, 0.5))),
        new Sphere(new PosVector(0.0, -100.5, -1.0), 100.0, new LambertianMaterial(new ColorVector(0.8, 0.8, 0.0))),
        new Sphere(new PosVector(1.0, 0.0, -1.0), 0.5, new MetalMaterial(new ColorVector(0.8, 0.6, 0.2), 0.3)),
        new Sphere(new PosVector(-1.0, 0.0, -1.0), 0.5, new DialectricMaterial(1.5)),
        new Sphere(new PosVector(-1.0, 0.0, -1.0), -0.45, new DialectricMaterial(1.5)),
      };

      var world = new HitableList
      {
        hitables
      };

      for (int j = height - 1; j >= 0; j--)
      {
        for (int i = 0; i < width; i++)
        {
          ColorVector color = new ColorVector(0.0, 0.0, 0.0);
          for (int sample = 0; sample < _numSamples; sample++)
          {
            double u = Convert.ToDouble(i + RandomService.NextDouble()) / Convert.ToDouble(width);
            double v = Convert.ToDouble(j + RandomService.NextDouble()) / Convert.ToDouble(height);
            var r = camera.GetRay(u, v);

            color += GetRayColor(r, world, 0);
          }

          color /= Convert.ToDouble(_numSamples);
          color = color.ApplyGamma2();

          pixelBuffer.SetPixelColor(i, j, color);
        }

        Console.Write(".");
      }

      Console.WriteLine();
      return pixelBuffer;
    }

    private ColorVector GetRayColor(Ray ray, IHitable world, int depth)
    {
      // the 0.001 corrects for the "shadow acne"
      HitRecord hr = world.Hit(ray, 0.001, double.MaxValue);
      if (hr != null)
      {
        if (depth < 50)
        {
          var scatterResult = hr.Material.Scatter(ray, hr);
          if (scatterResult.IsScattered)
          {
            return scatterResult.Attenuation * GetRayColor(scatterResult.ScatteredRay, world, depth + 1);
          }
        }

        return ColorVector.Zero;
      }
      else
      {
        var unitDirection = ray.Direction.ToUnitVector();
        double t = 0.5 * (unitDirection.Y + 1.0);
        return (((1.0 - t) * PosVector.One) + t * new PosVector(0.5, 0.7, 1.0)).ToColorVector();
      }
    }
  }
}