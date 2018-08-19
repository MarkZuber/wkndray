// -----------------------------------------------------------------------
// <copyright file="Sphere.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Materials;

namespace WkndRay
{
  public class Sphere : IHitable
  {
    public Sphere(PosVector center, double radius, IMaterial material = null)
    {
      Center = center;
      Radius = radius;
      Material = material;
    }

    public PosVector Center { get; }
    public double Radius { get; }
    public IMaterial Material { get; }

    public HitRecord Hit(Ray ray, double tMin, double tMax)
    {
      var oc = ray.Origin - Center;
      double a = ray.Direction.Dot(ray.Direction);
      double b = oc.Dot(ray.Direction);
      double c = oc.Dot(oc) - Radius * Radius;
      double discriminant = b * b - a * c;
      if (discriminant > 0.0)
      {
        double temp = (-b - Math.Sqrt(b * b - a * c)) / a;
        if (temp < tMax & temp > tMin)
        {
          var p = ray.GetPointAtParameter(temp);
          return new HitRecord(temp, p, (p - Center) / Radius, Material);
        }

        temp = (-b + Math.Sqrt(b * b - a * c)) / a;
        if (temp < tMax && temp > tMin)
        {
          var p = ray.GetPointAtParameter(temp);
          return new HitRecord(temp, p, (p - Center) / Radius, Material);
        }
      }

      return null;
    }

    /// <inheritdoc />
    public AABB GetBoundingBox(double t0, double t1)
    {
      return new AABB(Center - new PosVector(Radius, Radius, Radius), Center + new PosVector(Radius, Radius, Radius));
    }
  }
}