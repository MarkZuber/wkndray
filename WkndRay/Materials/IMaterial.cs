// -----------------------------------------------------------------------
// <copyright file="IMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System.Numerics;

namespace WkndRay.Materials
{
    public interface IMaterial
    {
        ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);
        float ScatteringPdf(Ray rayIn, HitRecord hitRecord, Ray scattered);
        ColorVector Emitted(Ray rayIn, HitRecord hitRecord, Point2D uvCoords, Vector3 p);
    }
}
