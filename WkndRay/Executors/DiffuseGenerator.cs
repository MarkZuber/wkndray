using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Executors
{
  public class DiffuseGenerator : IExecutor
  {
    private readonly IRandomService _randomService;
    private readonly int _numSamples;
    public DiffuseGenerator(IRandomService randomService, int numSamples)
    {
      _numSamples = numSamples;
      _randomService = randomService;
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
        new Sphere(new PosVector(0.0, 0.0, -1.0), 0.5),
        new Sphere(new PosVector(0.0, -100.5, -1.0), 100.0)
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
            double u = Convert.ToDouble(i + GetRandom()) / Convert.ToDouble(width);
            double v = Convert.ToDouble(j + GetRandom()) / Convert.ToDouble(height);
            var r = camera.GetRay(u, v);

            color += GetRayColor(r, world);
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

    private double GetRandom()
    {
      return _randomService.NextDouble();
    }

    private ColorVector GetRayColor(Ray ray, IHitable world)
    {
      // the 0.001 corrects for the "shadow acne"
      HitRecord hr = world.Hit(ray, 0.001, double.MaxValue);
      if (hr != null)
      {
        var target = hr.P + hr.Normal + PosVector.GetRandomInUnitSphere(_randomService);
        return (0.5 * GetRayColor(new Ray(hr.P, target - hr.P), world));
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
