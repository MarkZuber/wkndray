// -----------------------------------------------------------------------
// <copyright file="ShadedRaySphereGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Executors
{
    public class ShadedRaySphereGenerator : IExecutor
    {
        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);
            var lowerLeftCorner = new PosVector(-2.0f, -1.0f, -1.0f);
            var horizontal = new PosVector(4.0f, 0.0f, 0.0f);
            var vertical = new PosVector(0.0f, 2.0f, 0.0f);
            var origin = PosVector.Zero;

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
            float t = HitSphere(new PosVector(0.0f, 0.0f, -1.0f), 0.5f, ray);
            if (t > 0.0f)
            {
                var n = (ray.GetPointAtParameter(t) - new PosVector(0.0f, 0.0f, -1.0f)).ToUnitVector();
                return (0.5f * new PosVector(n.X + 1.0f, n.Y + 1.0f, n.Z + 1.0f)).ToColorVector();
            }

            PosVector unitDirection = ray.Direction.ToUnitVector();
            t = 0.5f * (unitDirection.Y + 1.0f);
            return (((1.0f - t) * PosVector.One) + (t * new PosVector(0.5f, 0.7f, 1.0f))).ToColorVector();
        }

        private float HitSphere(PosVector center, float radius, Ray ray)
        {
            var oc = ray.Origin - center;
            float a = ray.Direction.Dot(ray.Direction);
            float b = 2.0f * oc.Dot(ray.Direction);
            float c = oc.Dot(oc) - (radius * radius);
            float discriminant = (b * b) - (4 * a * c);
            if (discriminant < 0.0f)
            {
                return -1.0f;
            }
            else
            {
                return (-b - MathF.Sqrt(discriminant)) / (2.0f * a);
            }
        }
    }
}
