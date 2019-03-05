// -----------------------------------------------------------------------
// <copyright file="SimplePatternGenerator.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Executors
{
    public class SimplePatternGenerator : IExecutor
    {
        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);

            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    var color = new ColorVector(
                      Convert.ToDouble(i) / Convert.ToDouble(width),
                      Convert.ToDouble(j) / Convert.ToDouble(height),
                      0.2);
                    pixelBuffer.SetPixelColor(i, j, color);
                }
            }

            return pixelBuffer;
        }
    }
}