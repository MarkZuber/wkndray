// -----------------------------------------------------------------------
// <copyright file="IRayTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
  public interface IRayTracer
  {
    ColorVector GetPixelColor(int x, int y);
  }
}