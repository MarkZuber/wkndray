﻿// -----------------------------------------------------------------------
// <copyright file="AbstractMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
    public abstract class AbstractMaterial : IMaterial
    {
        public abstract ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);

        public virtual float ScatteringPdf(Ray rayIn, HitRecord hitRecord, Ray scattered)
        {
            return 0.0f;
        }

        public virtual ColorVector Emitted(Ray rayIn, HitRecord hitRecord, Point2D uvCoords, PosVector p)
        {
            return ColorVector.Zero;
        }
    }
}