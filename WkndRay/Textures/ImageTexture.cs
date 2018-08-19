// -----------------------------------------------------------------------
// <copyright file="ImageTexture.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay.Textures
{
  public class ImageTexture : ITexture
  {
    private readonly IPixelBuffer _pixelBuffer;

    public ImageTexture(IPixelBuffer pixelBuffer)
    {
      _pixelBuffer = pixelBuffer;
    }

    public int Width => _pixelBuffer.Width;
    public int Height => _pixelBuffer.Height;

    public ColorVector GetValue(Point2D uvCoords, PosVector p)
    {
      int i = Convert.ToInt32(uvCoords.U * Convert.ToDouble(Width));
      int j = Convert.ToInt32((1.0 - uvCoords.V) * Convert.ToDouble(Height) - 0.001);
      if (i < 0)
      {
        i = 0;
      }

      if (j < 0)
      {
        j = 0;
      }

      if (i > Width - 1)
      {
        i = Width - 1;
      }

      if (j > Height - 1)
      {
        j = Height - 1;
      }

      return _pixelBuffer.GetPixelColor(i, j);
    }
  }
}