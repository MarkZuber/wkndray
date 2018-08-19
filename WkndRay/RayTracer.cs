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
    private readonly Func<Ray, ColorVector> _backgroundFunc;

    public RayTracer(Camera camera, IHitable world, RenderConfig renderConfig, int imageWidth, int imageHeight, Func<Ray, ColorVector> backgroundFunc)
    {
      _camera = camera;
      _world = world;
      _renderConfig = renderConfig;
      _imageWidth = Convert.ToDouble(imageWidth);
      _imageHeight = Convert.ToDouble(imageHeight);
      _backgroundFunc = backgroundFunc;
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
        var emitted = hr.Material.Emitted(hr.UvCoords, hr.P);
        if (depth < _renderConfig.RayTraceDepth)
        {
          var scatterResult = hr.Material.Scatter(ray, hr);
          if (scatterResult.IsScattered)
          {
            return emitted + scatterResult.Attenuation * GetRayColor(scatterResult.ScatteredRay, world, depth + 1);
          }

          return emitted;
        }

        return emitted;
      }

      return _backgroundFunc(ray);
    }
  }
}