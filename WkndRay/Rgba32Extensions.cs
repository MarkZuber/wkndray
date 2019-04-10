// -----------------------------------------------------------------------
// <copyright file="Rgba32Extensions.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using SixLabors.ImageSharp.PixelFormats;

namespace WkndRay
{
    public static class Rgba32Extensions
    {
        public static ColorVector ToColorVector(this Rgba32 rgba32)
        {
            return new ColorVector(ByteToColor(rgba32.R), ByteToColor(rgba32.G), ByteToColor(rgba32.B));
        }

        private static float ByteToColor(byte c)
        {
            return Convert.ToSingle(c) / 255.0f;
        }
    }
}