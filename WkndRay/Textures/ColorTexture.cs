using System;

namespace WkndRay.Textures
{
  public class ColorTexture : ITexture
  {
    public ColorTexture(double r, double g, double b) : this(new ColorVector(r, g, b))
    {
    }

    public ColorTexture(ColorVector color)
    {
      Color = color;
    }

    public ColorVector Color { get; }

    public ColorVector GetValue(Point2D uvCoords, PosVector p)
    {
      return Color;
    }
  }
}
