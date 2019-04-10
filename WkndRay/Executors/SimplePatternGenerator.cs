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
                      Convert.ToSingle(i) / Convert.ToSingle(width),
                      Convert.ToSingle(j) / Convert.ToSingle(height),
                      0.2f);
                    pixelBuffer.SetPixelColor(i, j, color);
                }
            }

            return pixelBuffer;
        }
    }
}
