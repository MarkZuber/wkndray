using System;
using System.Collections.Generic;
using System.Text;

namespace WkndRay.Executors
{
  public class SimpleRayImageGenerator : IExecutor
  {
    public PixelBuffer Execute(int width, int height)
    {
      var pixelBuffer = new PixelBuffer(width, height);
      var lowerLeftCorner = new PosVector(-2.0, -1.0, -1.0);
      var horizontal = new PosVector(4.0, 0.0, 0.0);
      var vertical = new PosVector(0.0, 2.0, 0.0);
      var origin = PosVector.Zero;

      for (int j = height - 1; j >= 0; j--)
      {
        for (int i = 0; i < width; i++)
        {
          double u = Convert.ToDouble(i) / Convert.ToDouble(width);
          double v = Convert.ToDouble(j) / Convert.ToDouble(height);
          var r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
          var color = GetRayColor(r);
          pixelBuffer.SetPixelColor(i, j, color);
        }
      }

      return pixelBuffer;
    }

    private ColorVector GetRayColor(Ray ray)
    {
      PosVector unitDirection = ray.Direction.ToUnitVector();
      double t = 0.5 * (unitDirection.Y + 1.0);
      return (1.0 - t) * ColorVector.One + t * new ColorVector(0.5, 0.7, 1.0);
    }
  }
}
