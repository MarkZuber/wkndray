// -----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
  public class RayTracer : IRayTracer
  {
    private readonly Camera _camera;
    private readonly double _imageHeight;
    private readonly double _imageWidth;
    private readonly RenderConfig _renderConfig;
    private readonly IHitable _world;

    public RayTracer(Camera camera, IHitable world, RenderConfig renderConfig, int imageWidth, int imageHeight)
    {
      _camera = camera;
      _world = world;
      _renderConfig = renderConfig;
      _imageWidth = Convert.ToDouble(imageWidth);
      _imageHeight = Convert.ToDouble(imageHeight);
    }

    /// <inheritdoc />
    public ColorVector GetPixelColor(int x, int y)
    {
      ColorVector color = new ColorVector(0.0, 0.0, 0.0);
      double xDouble = Convert.ToDouble(x);
      double yDouble = Convert.ToDouble(y);
      if (_renderConfig.NumSamples > 1)
      {
        for (int sample = 0; sample < _renderConfig.NumSamples; sample++)
        {
          double u = (xDouble + RandomService.NextDouble()) / _imageWidth;
          double v = (yDouble + RandomService.NextDouble()) / _imageHeight;
          var r = _camera.GetRay(u, v);

          color += GetRayColor(r, _world, 0);
        }

        color /= Convert.ToDouble(_renderConfig.NumSamples);
      }
      else
      {
        color = GetRayColor(_camera.GetRay(xDouble / _imageWidth, yDouble / _imageHeight), _world, 0);
      }

      color = color.ApplyGamma2();
      return color;
    }

    private ColorVector GetRayColor(Ray ray, IHitable world, int depth)
    {
      // the 0.001 corrects for the "shadow acne"
      HitRecord hr = world.Hit(ray, 0.001, double.MaxValue);
      if (hr != null)
      {
        if (depth < _renderConfig.RayTraceDepth)
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
        // this is our background
        // todo: abstract this out so we can configure the background coloring...
        var unitDirection = ray.Direction.ToUnitVector();
        double t = 0.5 * (unitDirection.Y + 1.0);
        return (((1.0 - t) * ColorVector.One) + t * new ColorVector(0.5, 0.7, 1.0));
      }
    }
  }
}