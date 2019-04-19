﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WkndRay
{
    public static class Vector3Extensions
    {
        public static Vector3 ToUnitVector(this Vector3 vec)
        {
            float k = 1.0f / vec.Magnitude();
            return vec * k;
        }

        public static float Magnitude(this Vector3 vec)
        {
            return vec.Length();
        }

        public static float MagnitudeSquared(this Vector3 vec)
        {
            return vec.LengthSquared();
        }

        public static float Dot(this Vector3 vec, Vector3 other)
        {
            return Vector3.Dot(vec, other);
        }

        public static Vector3 Cross(this Vector3 vec, Vector3 other)
        {
            return Vector3.Cross(vec, other);
        }

        public static ColorVector ToColorVector(this Vector3 vec)
        {
            return new ColorVector(vec.X, vec.Y, vec.Z);
        }

        public static float[] ToSingleArray(this Vector3 vec)
        {
            return new float[] { vec.X, vec.Y, vec.Z };
        }

        public static Vector3 GetRandomInUnitSphere()
        {
            Vector3 pv;
            do
            {
                pv = (2.0f * new Vector3(RandomService.Nextfloat(), RandomService.Nextfloat(), RandomService.Nextfloat())) -
                     Vector3.One;
            }
            while (pv.LengthSquared() >= 1.0f);

            return pv;
        }

        public static Vector3 Reflect(this Vector3 vec, Vector3 other)
        {
            return vec - (2.0f * Vector3.Dot(vec, other) * other);
        }

        public static Vector3 Refract(this Vector3 vec, Vector3 normal, float niOverNt)
        {
            var unitVector = vec.ToUnitVector();
            float dt = Vector3.Dot(unitVector, normal);
            float discriminant = 1.0f - (niOverNt * niOverNt * (1.0f - (dt * dt)));
            if (discriminant > 0.0f)
            {
                return (niOverNt * (unitVector - (normal * dt))) - (normal * MathF.Sqrt(discriminant));
            }

            return Vector3.Zero;
        }
    }
}
