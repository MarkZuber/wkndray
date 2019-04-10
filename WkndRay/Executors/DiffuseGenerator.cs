// -----------------------------------------------------------------------
// <copyright file="DiffuseGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Executors
{
    public class DiffuseGenerator : IExecutor
    {
        private readonly int _numSamples;

        public DiffuseGenerator(int numSamples)
        {
            _numSamples = numSamples;
        }

        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);
            var lowerLeftCorner = new PosVector(-2.0f, -1.0f, -1.0f);
            var horizontal = new PosVector(4.0f, 0.0f, 0.0f);
            var vertical = new PosVector(0.0f, 2.0f, 0.0f);
            var origin = PosVector.Zero;

            var camera = new BasicCamera(origin, lowerLeftCorner, horizontal, vertical);

            var hitables = new HitableList
            {
                new Sphere(new PosVector(0.0f, 0.0f, -1.0f), 0.5f),
                new Sphere(new PosVector(0.0f, -100.5f, -1.0f), 100.0f)
            };

            var world = new HitableList
            {
                hitables
            };

            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    ColorVector color = new ColorVector(0.0f, 0.0f, 0.0f);
                    for (int sample = 0; sample < _numSamples; sample++)
                    {
                        float u = Convert.ToSingle(i + RandomService.Nextfloat()) / Convert.ToSingle(width);
                        float v = Convert.ToSingle(j + RandomService.Nextfloat()) / Convert.ToSingle(height);
                        var r = camera.GetRay(u, v);

                        color += GetRayColor(r, world);
                    }

                    color /= Convert.ToSingle(_numSamples);
                    color = color.ApplyGamma2();

                    pixelBuffer.SetPixelColor(i, j, color);
                }

                Console.Write(".");
            }

            Console.WriteLine();
            return pixelBuffer;
        }

        private ColorVector GetRayColor(Ray ray, IHitable world)
        {
            // the 0.001 corrects for the "shadow acne"
            HitRecord hr = world.Hit(ray, 0.001f, float.MaxValue);
            if (hr != null)
            {
                var target = hr.P + hr.Normal + PosVector.GetRandomInUnitSphere();
                return 0.5f * GetRayColor(new Ray(hr.P, target - hr.P), world);
            }
            else
            {
                var unitDirection = ray.Direction.ToUnitVector();
                float t = 0.5f * (unitDirection.Y + 1.0f);
                return (((1.0f - t) * PosVector.One) + (t * new PosVector(0.5f, 0.7f, 1.0f))).ToColorVector();
            }
        }
    }
}
