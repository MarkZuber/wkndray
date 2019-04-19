// -----------------------------------------------------------------------
// <copyright file="HitRecord.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System.Numerics;
using WkndRay.Materials;

namespace WkndRay
{
    public class HitRecord
    {
        public HitRecord(float t, Vector3 p, Vector3 normal, Point2D uvCoords, IMaterial material)
        {
            T = t;
            P = p;
            Normal = normal;
            Material = material;
            UvCoords = uvCoords ?? new Point2D(0.0f, 0.0f);
        }

        public float T { get; }
        public Vector3 P { get; }
        public Vector3 Normal { get; }
        public IMaterial Material { get; }

        // Texture Coordinates
        public Point2D UvCoords { get; }
    }
}
