﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Executors
{
  public class FirstCameraTracer : IExecutor
  {
    private readonly Random _random = new Random();
    private readonly int _numSamples;
    public FirstCameraTracer(int numSamples)
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

          pixelBuffer.SetPixelColor(i, j, color);
        }
        Console.Write(".");
      }

      Console.WriteLine();
      return pixelBuffer;
    }

    private double GetRandom()
    {
      return _random.NextDouble();
    }

    private ColorVector GetRayColor(Ray ray, IHitable world)
    {
      HitRecord hr = world.Hit(ray, 0.0, double.MaxValue);
      if (hr != null)
      {
        return (0.5 * new PosVector(hr.Normal.X + 1.0, hr.Normal.Y + 1.0, hr.Normal.Z + 1.0)).ToColorVector();
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
