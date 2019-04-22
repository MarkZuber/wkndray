// -----------------------------------------------------------------------
// <copyright file="SimpleRaySphereGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;

namespace WkndRay.Executors
{
    public class SimpleRaySphereGenerator : IExecutor
    {
        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);
            var lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            var horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            var vertical = new Vector3(0.0f, 2.0f, 0.0f);
            var origin = Vector3.Zero;

            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    float u = Convert.ToSingle(i) / Convert.ToSingle(width);
                    float v = Convert.ToSingle(j) / Convert.ToSingle(height);
                    var r = new Ray(origin, lowerLeftCorner + (u * horizontal) + (v * vertical));
                    var color = GetRayColor(r);
                    pixelBuffer.SetPixelColor(i, j, color);
                }
            }

            return pixelBuffer;
        }

        private ColorVector GetRayColor(Ray ray)
        {
            if (IsHitSphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, ray))
            {
                return new ColorVector(1.0f, 0.0f, 0.0f);
            }

            Vector3 unitDirection = ray.Direction.ToUnitVector();
            float t = 0.5f * (unitDirection.Y + 1.0f);
            var pv = ((1.0f - t) * Vector3.One) + (t * new Vector3(0.5f, 0.7f, 1.0f));
            return new ColorVector(pv.X, pv.Y, pv.Z);
        }

        private bool IsHitSphere(Vector3 center, float radius, Ray ray)
        {
            var oc = ray.Origin - center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - (radius * radius);
            float discriminant = (b * b) - (4 * a * c);
            return discriminant > 0;
        }
    }
}
