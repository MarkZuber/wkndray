// -----------------------------------------------------------------------
// <copyright file="SimpleRayImageGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;

namespace WkndRay.Executors
{
    public class SimpleRayImageGenerator : IExecutor
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
            Vector3 unitDirection = ray.Direction.ToUnitVector();
            float t = 0.5f * (unitDirection.Y + 1.0f);
            return ((1.0f - t) * ColorVector.One) + (t * new ColorVector(0.5f, 0.7f, 1.0f));
        }
    }
}
