using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace WkndRay
{
  public class PixelBuffer
  {
    private readonly Image<Rgba32> _image;
    private readonly object _lock = new object();

    public PixelBuffer(int width, int height)
    {
      _image = new Image<Rgba32>(width, height);
    }

    public int Width => _image.Width;
    public int Height => _image.Height;

    public void SetPixelColor(int x, int y, Vector3 color)
    {
      SetPixelColor(x, y, color.ToRgba32());
    }

    private void SetPixelColor(int x, int y, Rgba32 rgba)
    {
      lock (_lock)
      {
        _image[x, y] = rgba;
      }
    }

    public void SetPixelColor(int x, int y, byte r, byte g, byte b)
    {
      SetPixelColor(x, y, new Rgba32(r, g, b));
    }

    public void SaveAsFile(string outputFilePath)
    {
      lock (_lock)
      {
        _image.Save(outputFilePath);
      }
    }
  }
}
