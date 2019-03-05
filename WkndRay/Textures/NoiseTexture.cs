// -----------------------------------------------------------------------
// <copyright file="NoiseTexture.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Textures
{
    public class NoiseTexture : ITexture
    {
        private readonly Perlin _noise = new Perlin();

        public NoiseTexture(bool interpolate, double scale)
        {
            Interpolate = interpolate;
            Scale = scale;
        }

        public bool Interpolate { get; }
        public double Scale { get; }

        public ColorVector GetValue(Point2D uvCoords, PosVector p)
        {
            return ColorVector.One * _noise.Noise(Scale * p, Interpolate);
        }
    }
}