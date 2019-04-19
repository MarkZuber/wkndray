// -----------------------------------------------------------------------
// <copyright file="DiffuseLight.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System.Numerics;
using WkndRay.Textures;

namespace WkndRay.Materials
{
    public class DiffuseLight : AbstractMaterial
    {
        public DiffuseLight(ITexture texture)
        {
            Texture = texture;
        }

        public ITexture Texture { get; }

        public override ScatterResult Scatter(Ray rayIn, HitRecord hitRecord)
        {
            return ScatterResult.False();
        }

        public override ColorVector Emitted(Ray rayIn, HitRecord hitRecord, Point2D uvCoords, Vector3 p)
        {
            return hitRecord.Normal.Dot(rayIn.Direction) < 0.0f ? Texture.GetValue(uvCoords, p) : Texture.GetValue(uvCoords, p);
        }
    }
}
