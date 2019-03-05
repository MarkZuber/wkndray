// -----------------------------------------------------------------------
// <copyright file="Sphere.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using WkndRay.Hitables;
using WkndRay.Materials;

namespace WkndRay
{
    public class Sphere : AbstractHitable
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

        public override HitRecord Hit(Ray ray, double tMin, double tMax)
        {
            var oc = ray.Origin - Center;
            double a = ray.Direction.Dot(ray.Direction);
            double b = oc.Dot(ray.Direction);
            double c = oc.Dot(oc) - (Radius * Radius);
            double discriminant = (b * b) - (a * c);
            if (discriminant > 0.0)
            {
                double temp = (-b - Math.Sqrt((b * b) - (a * c))) / a;
                if (temp < tMax & temp > tMin)
                {
                    var p = ray.GetPointAtParameter(temp);
                    return new HitRecord(temp, p, (p - Center) / Radius, GetSphereUv(p), Material);
                }

                temp = (-b + Math.Sqrt((b * b) - (a * c))) / a;
                if (temp < tMax && temp > tMin)
                {
                    var p = ray.GetPointAtParameter(temp);
                    return new HitRecord(temp, p, (p - Center) / Radius, GetSphereUv(p), Material);
                }
            }

            return null;
        }

        public override AABB GetBoundingBox(double t0, double t1)
        {
            return new AABB(Center - new PosVector(Radius, Radius, Radius), Center + new PosVector(Radius, Radius, Radius));
        }

        public override double GetPdfValue(PosVector origin, PosVector v)
        {
            HitRecord hr = Hit(new Ray(origin, v), 0.001, double.MaxValue);
            if (hr == null)
            {
                return 0.0;
            }

            double cosThetaMax = Math.Sqrt(1.0 - (Radius * Radius / (Center - origin).MagnitudeSquared()));
            double solidAngle = 2.0 * Math.PI * (1.0 - cosThetaMax);
            return 1.0 / solidAngle;
        }

        public override PosVector Random(PosVector origin)
        {
            PosVector direction = Center - origin;
            double distanceSquared = direction.MagnitudeSquared();
            OrthoNormalBase uvw = OrthoNormalBase.FromW(direction);
            return uvw.Local(RandomService.RandomToSphere(Radius, distanceSquared));
        }

        private Point2D GetSphereUv(PosVector p)
        {
            var punit = p.ToUnitVector();
            double phi = Math.Atan2(punit.Z, punit.X);
            double theta = Math.Asin(punit.Y);
            double u = 1.0 - ((phi + Math.PI) / (2.0 * Math.PI));
            double v = (theta + (Math.PI / 2.0)) / Math.PI;
            return new Point2D(u, v);
        }
    }
}