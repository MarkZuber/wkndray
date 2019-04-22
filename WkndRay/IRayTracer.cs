// -----------------------------------------------------------------------
// <copyright file="IRayTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
    public interface IRayTracer
    {
        PixelData GetPixelColor(int x, int y);
    }
}
