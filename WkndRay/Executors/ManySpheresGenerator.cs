// -----------------------------------------------------------------------
// <copyright file="ManySpheresGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Executors
{
    public class ManySpheresGenerator : IExecutor
    {
        private readonly int _numSamples;

        public ManySpheresGenerator(int numSamples)
        {
            _numSamples = numSamples;
        }

        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);
            double aperture = 0.01;
            var lookFrom = new PosVector(24.0, 2.0, 6.0);
            var lookAt = PosVector.UnitY;
            double distanceToFocus = (lookFrom - lookAt).Magnitude();
            var camera = new Camera(
              lookFrom,
              lookAt,
              PosVector.UnitY,
              15.0,
              Convert.ToDouble(width) / Convert.ToDouble(height),
              aperture,
              distanceToFocus);

            var world = CreateRandomScene();

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

        private IHitable CreateRandomScene()
        {
            var list = new HitableList
            {
                new Sphere(new PosVector(0.0, -1000.0, 0.0), 1000.0, new LambertianMaterial(new ColorTexture(0.5, 0.5, 0.5)))
            };
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMat = RandomService.NextDouble();
                    var center = new PosVector(
                      Convert.ToDouble(a) * RandomService.NextDouble(),
                      0.2,
                      Convert.ToDouble(b) + (0.9 * RandomService.NextDouble()));
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
                                  new ColorTexture(
                                    RandomService.NextDouble() * RandomService.NextDouble(),
                                    RandomService.NextDouble() * RandomService.NextDouble(),
                                    RandomService.NextDouble() * RandomService.NextDouble()))));
                        }
                        else if (chooseMat < 0.95)
                        {
                            // metal
                            list.Add(
                              new Sphere(
                                center,
                                0.2,
                                new MetalMaterial(
                                  new ColorVector(
                                    0.5 * (1.0 + RandomService.NextDouble()),
                                    0.5 * (1.0 + RandomService.NextDouble()),
                                    0.5 * (1.0 + RandomService.NextDouble())),
                                  0.5 * RandomService.NextDouble())));
                        }
                        else
                        {
                            // glass
                            list.Add(new Sphere(center, 0.2, new DialectricMaterial(1.5)));
                        }
                    }
                }
            }

            list.Add(new Sphere(new PosVector(0.0, 1.0, 0.0), 1.0, new DialectricMaterial(1.5)));
            list.Add(new Sphere(new PosVector(-4.0, 1.0, 0.0), 1.0, new LambertianMaterial(new ColorTexture(0.4, 0.2, 0.1))));
            list.Add(new Sphere(new PosVector(4.0, 1.0, 0.0), 1.0, new MetalMaterial(new ColorVector(0.7, 0.6, 0.5), 0.0)));

            return list;
        }

        private double GetRandom()
        {
            return RandomService.NextDouble();
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
                        return scatterResult.Attenuation * GetRayColor(scatterResult.SpecularRay, world, depth + 1);
                    }
                }

                return ColorVector.Zero;
            }
            else
            {
                var unitDirection = ray.Direction.ToUnitVector();
                double t = 0.5 * (unitDirection.Y + 1.0);
                return (((1.0 - t) * PosVector.One) + (t * new PosVector(0.5, 0.7, 1.0))).ToColorVector();
            }
        }
    }
}