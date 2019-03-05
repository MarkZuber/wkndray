// -----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Pdfs;

namespace WkndRay
{
    public class RayTracer : IRayTracer
    {
        private readonly Func<Ray, ColorVector> _backgroundFunc;
        private readonly Camera _camera;
        private readonly double _imageHeight;
        private readonly double _imageWidth;
        private readonly IHitable _lightHitable;
        private readonly RenderConfig _renderConfig;
        private readonly IHitable _world;

        public RayTracer(
          Camera camera,
          IHitable world,
          IHitable lightHitable,
          RenderConfig renderConfig,
          int imageWidth,
          int imageHeight,
          Func<Ray, ColorVector> backgroundFunc)
        {
            _camera = camera;
            _world = world;
            _lightHitable = lightHitable;
            _renderConfig = renderConfig;
            _imageWidth = Convert.ToDouble(imageWidth);
            _imageHeight = Convert.ToDouble(imageHeight);
            _backgroundFunc = backgroundFunc;
        }

        /// <inheritdoc />
        public ColorVector GetPixelColor(int x, int y)
        {
            ColorVector color = ColorVector.Zero;
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
                var emitted = hr.Material.Emitted(ray, hr, hr.UvCoords, hr.P);
                if (depth < _renderConfig.RayTraceDepth)
                {
                    var scatterResult = hr.Material.Scatter(ray, hr);
                    if (scatterResult.IsScattered)
                    {
                        if (scatterResult.IsSpecular)
                        {
                            return scatterResult.Attenuation * GetRayColor(scatterResult.SpecularRay, world, depth + 1);
                        }
                        else
                        {
                            var p0 = new HitablePdf(_lightHitable, hr.P);
                            var p = new MixturePdf(p0, scatterResult.Pdf);
                            var scattered = new Ray(hr.P, p.Generate());
                            double pdfValue = p.GetValue(scattered.Direction);
                            return emitted + (scatterResult.Attenuation * hr.Material.ScatteringPdf(ray, hr, scattered) *
                                   GetRayColor(scattered, world, depth + 1) / pdfValue);
                        }
                    }
                }

                return emitted;
            }

            return _backgroundFunc(ray);
        }
    }
}