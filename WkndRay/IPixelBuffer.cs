﻿// -----------------------------------------------------------------------
// <copyright file="IPixelBuffer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace WkndRay
{
  public interface IPixelBuffer
  {
    int Width { get; }
    int Height { get; }

    /// <summary>
    ///   Our Y axis is UP (right handed coordinate system)
    ///   X is right, and positive Z is out of the screen towards
    ///   the viewer.  So our calculated Y pixels are
    ///   the opposite direction of the Y in the image buffer.
    ///   If IsYUp is true then we'll invert Y when setting it into
    ///   the image.
    /// </summary>
    bool IsYUp { get; }

    void SetPixelColor(int x, int y, ColorVector color);
    void SetPixelColor(int x, int y, byte r, byte g, byte b);
    void SaveAsFile(string outputFilePath);
    void SetPixelRowColors(int y, IEnumerable<ColorVector> rowPixels);
  }
}