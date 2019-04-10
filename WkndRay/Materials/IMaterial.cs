// -----------------------------------------------------------------------
// <copyright file="IMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
    public interface IMaterial
    {
        ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);
        float ScatteringPdf(Ray rayIn, HitRecord hitRecord, Ray scattered);
        ColorVector Emitted(Ray rayIn, HitRecord hitRecord, Point2D uvCoords, PosVector p);
    }
}