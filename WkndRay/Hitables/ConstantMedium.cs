﻿// -----------------------------------------------------------------------
// <copyright file="ConstantMedium.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;
using WkndRay.Materials;
using WkndRay.Textures;

namespace WkndRay.Hitables
{
    public class ConstantMedium : AbstractHitable
    {
        public ConstantMedium(IHitable boundary, float density, ITexture a)
        {
            Boundary = boundary;
            Density = density;
            PhaseFunction = new IsotropicMaterial(a);
        }

        public IHitable Boundary { get; }
        public float Density { get; }
        public IMaterial PhaseFunction { get; }

        public override HitRecord Hit(Ray ray, float tMin, float tMax)
        {
            HitRecord hitRecord1 = Boundary.Hit(ray, -float.MaxValue, float.MaxValue);
            if (hitRecord1 == null)
            {
                return null;
            }

            HitRecord hitRecord2 = Boundary.Hit(ray, hitRecord1.T + 0.0001f, float.MaxValue);
            if (hitRecord2 == null)
            {
                return null;
            }

            float rec1T = hitRecord1.T;
            float rec2T = hitRecord2.T;

            if (rec1T < tMin)
            {
                rec1T = tMin;
            }

            if (rec2T > tMax)
            {
                rec2T = tMax;
            }

            if (rec1T >= rec2T)
            {
                return null;
            }

            if (rec1T < 0.0f)
            {
                rec1T = 0.0f;
            }

            float distanceInsideBoundary = ((rec2T - rec1T) * ray.Direction).Magnitude();
            float hitDistance = -(1.0f / Density) * MathF.Log(RandomService.Nextfloat());
            if (hitDistance < distanceInsideBoundary)
            {
                float recT = rec1T + (hitDistance / ray.Direction.Magnitude());

                return new HitRecord(
                  recT,
                  ray.GetPointAtParameter(recT),
                  Vector3.UnitX,  // arbitrary
                  new Point2D(0.0f, 0.0f), // don't need u/v since PhaseFunction is a calculation
                  PhaseFunction);
            }

            return null;
        }

        public override AABB GetBoundingBox(float t0, float t1)
        {
            return Boundary.GetBoundingBox(t0, t1);
        }
    }
}
