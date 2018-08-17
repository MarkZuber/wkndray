// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;

namespace WkndRay
{
  public class SimplePatternGenerator
  {
    public PixelBuffer Execute(int width, int height)
    {
      var pixelBuffer = new PixelBuffer(width, height);

      for (int j = height - 1; j >= 0; j--)
      {
        for (int i = 0; i < width; i++)
        {
          var color = new Vector3((float)(i) / (float)(width), (float)(j) / (float)(height), 0.2f);
          pixelBuffer.SetPixelColor(i, j, color);
        }
      }

      return pixelBuffer;
    }
  }
}