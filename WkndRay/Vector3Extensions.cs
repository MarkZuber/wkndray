using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;

namespace WkndRay
{
  public static class Vector3Extensions
  {
    public static Vector3 ClampColor(this Vector3 v)
    {
      return Vector3.Clamp(v, Vector3.Zero, Vector3.One);
    }

    public static Rgba32 ToRgba32(this Vector3 v)
    {
      var v2 = v.ClampColor();
      return new Rgba32(ColorToByte(v2.X), ColorToByte(v2.Y), ColorToByte(v2.Z));
    }

    private static byte ColorToByte(float c)
    {
      return Convert.ToByte(c * 255.99);
    }
  }
}
