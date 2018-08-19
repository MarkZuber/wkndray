// -----------------------------------------------------------------------
// <copyright file="MaterialGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Executors
{
  public class MaterialGenerator : IExecutor
  {
    private readonly int _numSamples;

    public MaterialGenerator(int numSamples)
    {
      _numSamples = numSamples;
    }

    public PixelBuffer Execute(int width, int height)
    {
      var pixelBuffer = new PixelBuffer(width, height);
      var lowerLeftCorner = new PosVector(-2.0, -1.0, -1.0);
      var horizontal = new PosVector(4.0, 0.0, 0.0);
      var vertical = new PosVector(0.0, 2.0, 0.0);
      var origin = PosVector.Zero;

      var camera = new BasicCamera(origin, lowerLeftCorner, horizontal, vertical);

      var hitables = new HitableList
      {
        new Sphere(new PosVector(0.0, 0.0, -1.0), 0.5, new LambertianMaterial(new ColorTexture(0.8, 0.3, 0.3))),
        new Sphere(new PosVector(0.0, -100.5, -1.0), 100.0, new LambertianMaterial(new ColorTexture(0.8, 0.8, 0.0))),
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