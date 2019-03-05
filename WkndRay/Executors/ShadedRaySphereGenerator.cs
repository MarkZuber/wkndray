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
                    var r = new Ray(origin, lowerLeftCorner + (u * horizontal) + (v * vertical));
                    var color = GetRayColor(r);
                    pixelBuffer.SetPixelColor(i, j, color);
                }
            }

            return pixelBuffer;
        }

        private ColorVector GetRayColor(Ray ray)
        {
            double t = HitSphere(new PosVector(0.0, 0.0, -1.0), 0.5, ray);
            if (t > 0.0)
            {
                var n = (ray.GetPointAtParameter(t) - new PosVector(0.0, 0.0, -1.0)).ToUnitVector();
                return (0.5 * new PosVector(n.X + 1.0, n.Y + 1.0, n.Z + 1.0)).ToColorVector();
            }

            PosVector unitDirection = ray.Direction.ToUnitVector();
            t = 0.5 * (unitDirection.Y + 1.0);
            return (((1.0 - t) * PosVector.One) + (t * new PosVector(0.5, 0.7, 1.0))).ToColorVector();
        }

        private double HitSphere(PosVector center, double radius, Ray ray)
        {
            var oc = ray.Origin - center;
            double a = ray.Direction.Dot(ray.Direction);
            double b = 2.0 * oc.Dot(ray.Direction);
            double c = oc.Dot(oc) - (radius * radius);
            double discriminant = (b * b) - (4 * a * c);
            if (discriminant < 0.0)
            {
                return -1.0;
            }
            else
            {
                return (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            }
        }
    }
}