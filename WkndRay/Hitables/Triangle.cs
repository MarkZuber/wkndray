// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using WkndRay.Materials;

namespace WkndRay.Hitables
{
    public class Triangle : AbstractHitable
    {
        public Triangle(IEnumerable<PosVector> vertices, IMaterial material)
        {
            var verts = vertices.ToList();
            if (verts.Count != 3)
            {
                throw new ArgumentException("triangle must have exactly 3 vertices");
            }

            Vertices = verts;
            SurfaceNormal = (Vertices[2] - Vertices[0]).Cross(Vertices[1] - Vertices[0]).ToUnitVector();
            Material = material;
        }

        public List<PosVector> Vertices { get; }
        public PosVector SurfaceNormal { get; }
        public IMaterial Material { get; }

        public override HitRecord Hit(Ray ray, float tMin, float tMax)
        {
            var e1 = Vertices[1] - Vertices[0];
            var e2 = Vertices[2] - Vertices[0];
            var dir = ray.Direction;

            var pvec = dir.Cross(e2);
            var det = e1.Dot(pvec);

            if (det > -0.0001 && det < 0.0001)
            {
                return null;
            }

            var invDet = 1.0f / det;
            var tvec = ray.Origin - Vertices[0];
            var u = tvec.Dot(pvec) * invDet;

            if (u < 0.0f || u > 1.0f)
            {
                return null;
            }

            var qvec = tvec.Cross(e1);
            var v = dir.Dot(qvec) * invDet;

            if (v < 0.0f || (u + v) > 1.0f)
            {
                return null;
            }

            var t = e2.Dot(qvec) * invDet;
            if (t > 0.00001 && t < tMax && t > tMin)
            {
                return new HitRecord(t, ray.GetPointAtParameter(t), SurfaceNormal, new Point2D(u, v), Material);
            }

            return null;
        }

        public override AABB GetBoundingBox(float t0, float t1)
        {
            var min = new PosVector(
              MathF.Min(Vertices[0].X, Vertices[1].X),
              MathF.Min(Vertices[0].Y, Vertices[1].Y),
              MathF.Min(Vertices[0].Z, Vertices[1].Z));
            min = new PosVector(
              MathF.Min(min.X, Vertices[2].X),
              MathF.Min(min.Y, Vertices[2].Y),
              MathF.Min(min.Z, Vertices[2].Z));

            var max = new PosVector(
              MathF.Max(Vertices[0].X, Vertices[1].X),
              MathF.Max(Vertices[0].Y, Vertices[1].Y),
              MathF.Max(Vertices[0].Z, Vertices[1].Z));
            max = new PosVector(
              MathF.Max(max.X, Vertices[2].X),
              MathF.Max(max.Y, Vertices[2].Y),
              MathF.Max(max.Z, Vertices[2].Z));

            return new AABB(min, max);
        }
    }
}