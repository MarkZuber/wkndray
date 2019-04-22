﻿// -----------------------------------------------------------------------
// <copyright file="RayTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using WkndRay.Materials;
using WkndRay.Pdfs;

namespace WkndRay
{
    public class RayTracer : IRayTracer
    {
        private readonly Func<Ray, ColorVector> _backgroundFunc;
        private readonly Camera _camera;
        private readonly float _imageHeight;
        private readonly float _imageWidth;
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
            _imageWidth = Convert.ToSingle(imageWidth);
            _imageHeight = Convert.ToSingle(imageHeight);
            _backgroundFunc = backgroundFunc;
        }

        /// <inheritdoc />
        public PixelData GetPixelColor(int x, int y)
        {
            Debug.WriteLine($"Pixel: ({x}, {y})");

            var pixelData = new PixelData(x, y);

            using (new BlockTimer(pixelData.SetPixelColorMilliseconds))
            {
                ColorVector color = ColorVector.Zero;
                float xfloat = Convert.ToSingle(x);
                float yfloat = Convert.ToSingle(y);

                long totalSampleMilliseconds = 0;

                if (_renderConfig.NumSamples > 1)
                {
                    for (int sample = 0; sample < _renderConfig.NumSamples; sample++)
                    {
                        var sw = Stopwatch.StartNew();
                        float u = (xfloat + RandomService.Nextfloat()) / _imageWidth;
                        float v = (yfloat + RandomService.Nextfloat()) / _imageHeight;
                        var r = _camera.GetRay(u, v);

                        color += GetRayColor(r, _world, pixelData, 0).DeNan();
                        sw.Stop();
                        totalSampleMilliseconds += sw.ElapsedMilliseconds;
                    }

                    color /= Convert.ToSingle(_renderConfig.NumSamples);
                }
                else
                {
                    var sw = Stopwatch.StartNew();
                    color = GetRayColor(_camera.GetRay(xfloat / _imageWidth, yfloat / _imageHeight), _world, pixelData, 0).DeNan();
                    totalSampleMilliseconds += sw.ElapsedMilliseconds;
                }

                pixelData.AverageSampleMilliseconds = totalSampleMilliseconds / _renderConfig.NumSamples;

                color = color.DeNan().ApplyGamma2();
                Debug.WriteLine($"Final Color at ({x}, {y}) -> ({color.R}, {color.G}, {color.B})");
                pixelData.Color = color;
            }

            return pixelData;
        }

        private ColorVector GetRayColor(Ray ray, IHitable world, PixelData pixelData, int depth)
        {
            Debug.WriteLine($"Depth: {depth}");
            pixelData.SetDepth(depth);
            try
            {
                // the 0.001 corrects for the "shadow acne"
                HitRecord hr = world.Hit(ray, 0.001f, float.MaxValue);
                if (hr != null)
                {
                    var emitted = hr.Material.Emitted(ray, hr, hr.UvCoords, hr.P);

                    if (hr.Material is DiffuseLight)
                    {
                        Debug.WriteLine($"HIT A LIGHT. Emitted: {emitted}");
                    }

                    if (depth < _renderConfig.RayTraceDepth)
                    {
                        var scatterResult = hr.Material.Scatter(ray, hr);
                        if (scatterResult.IsScattered)
                        {
                            if (scatterResult.IsSpecular)
                            {
                                return scatterResult.Attenuation * GetRayColor(scatterResult.SpecularRay, world, pixelData, depth + 1);
                            }
                            else
                            {
                                var p0 = new HitablePdf(_lightHitable, hr.P);
                                var p = new MixturePdf(p0, scatterResult.Pdf);
                                var scattered = new Ray(hr.P, p.Generate());
                                float pdfValue = p.GetValue(scattered.Direction);

                                var scatteringPdf = hr.Material.ScatteringPdf(ray, hr, scattered);
                                if (scatteringPdf < 0.01f)
                                {
                                    scatteringPdf = 0.01f;
                                //    //pdfValue = 1.0f;
                                }

                                {
                                    //pdfValue = 1.0f;
                                }

                                var depthRayColor = GetRayColor(scattered, world, pixelData, depth + 1);
                                ColorVector recurseColor = ((scatterResult.Attenuation * scatteringPdf * depthRayColor) / pdfValue);
                                Debug.WriteLine($"Attenuation ({scatterResult.Attenuation}) ScatteringPdf ({scatteringPdf}) DepthRayColor({depthRayColor}) PdfValue({pdfValue})");
                                Debug.WriteLine($"emitted: {emitted}");
                                Debug.WriteLine($"RecurseColor: {recurseColor}");
                                return emitted + recurseColor;
                            }
                        }
                        else
                        {
                            Debug.WriteLine("NOT SCATTERED");
                        }
                    }

                    return emitted;
                }

                if (depth == 0)
                {
                    Debug.WriteLine("depth at 0...");
                }

                Debug.WriteLine("returning backgroundfunc");
                return _backgroundFunc(ray);
            }
            finally
            {
                Debug.WriteLine($"Exiting Depth: {depth}");
            }
        }
    }
}
