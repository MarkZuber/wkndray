// -----------------------------------------------------------------------
// <copyright file="WpfPixelBuffer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WkndRay;

namespace RayWpf
{
  public class WpfPixelBuffer : IPixelBuffer
  {
    private readonly Dispatcher _dispatcher;

    // private readonly PixelBuffer _pixelBuffer;

    public WpfPixelBuffer(Dispatcher dispatcher, int width, int height, bool isYUp = true)
    {
      // _pixelBuffer = new PixelBuffer(width, height, isYUp);
      WriteableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
      _dispatcher = dispatcher;
      Width = width;
      Height = height;
      IsYUp = isYUp;
    }

    public WriteableBitmap WriteableBitmap { get; }

    //public int Width => _pixelBuffer.Width;
    //public int Height => _pixelBuffer.Height;
    //public bool IsYUp => _pixelBuffer.IsYUp;

    public int Width { get; }
    public int Height { get; }
    public bool IsYUp { get; }

    public void SetPixelColor(int x, int y, ColorVector color)
    {
      // _pixelBuffer.SetPixelColor(x, y, color);
      _dispatcher.Invoke(
        () =>
        {
          int yToSet = IsYUp ? (Height - 1 - y) : y;
          Lock();
          try
          {
            UnsafeSetPixelColor(x, yToSet, color);
            WriteableBitmap.AddDirtyRect(new Int32Rect(x, yToSet, 1, 1));
          }
          finally
          {
            Unlock();
          }
        });
    }

    public void SetPixelColor(int x, int y, byte r, byte g, byte b)
    {
      SetPixelColor(x, y, ColorVector.FromBytes(r, g, b));
    }

    public ColorVector GetPixelColor(int x, int y)
    {
      throw new NotImplementedException();
      // return _pixelBuffer.GetPixelColor(x, y);
    }

    public void SaveAsFile(string outputFilePath)
    {
      // _pixelBuffer.SaveAsFile(outputFilePath);
    }

    public void SetPixelRowColors(int y, IEnumerable<ColorVector> rowPixels)
    {
      var xPixels = rowPixels.ToList();
      // _pixelBuffer.SetPixelRowColors(y, xPixels);
      _dispatcher.Invoke(
        () =>
        {
          for (int x = 0; x < Width; x++)
          {
            UnsafeSetPixelColor(x, y, xPixels[x]);
          }

          WriteableBitmap.AddDirtyRect(new Int32Rect(0, y, Width, 1));
        });
    }

    public void Dispose()
    {
    }

    public void Lock()
    {
      _dispatcher.Invoke(() => { WriteableBitmap.Lock(); });
    }

    public void Unlock()
    {
      _dispatcher.Invoke(() => { WriteableBitmap.Unlock(); });
    }

    private int ColorToInt(double c)
    {
      return Convert.ToInt32(c * 255.0);
    }

    private void UnsafeSetPixelColor(int x, int y, ColorVector color)
    {
      unsafe
      {
        // Get a pointer to the back buffer.
        long pBackBuffer = (long)WriteableBitmap.BackBuffer;

        // Find the address of the pixel to draw.
        pBackBuffer += y * WriteableBitmap.BackBufferStride;
        pBackBuffer += x * 4;

        // Compute the pixel's color.
        var clamped = color.ClampColor();
        int colorData = ColorToInt(clamped.R) << 16; // R
        colorData |= ColorToInt(clamped.G) << 8; // G
        colorData |= ColorToInt(clamped.B) << 0; // B

        // Assign the color data to the pixel.
        *(long*)pBackBuffer = colorData;
      }
    }
  }
}