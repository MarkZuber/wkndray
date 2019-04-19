﻿// -----------------------------------------------------------------------
// <copyright file="FirstHitableTracer.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Numerics;

namespace WkndRay.Executors
{
    public class FirstHitableTracer : IExecutor
    {
        public PixelBuffer Execute(int width, int height)
        {
            var pixelBuffer = new PixelBuffer(width, height);
            var lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            var horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            var vertical = new Vector3(0.0f, 2.0f, 0.0f);
            var origin = Vector3.Zero;

            var hitables = new HitableList
      {
        new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f),
        new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f)
      };

            var world = new HitableList
      {
        hitables
      };

            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    float u = Convert.ToSingle(i) / Convert.ToSingle(width);
                    float v = Convert.ToSingle(j) / Convert.ToSingle(height);
                    var r = new Ray(origin, lowerLeftCorner + (u * horizontal) + (v * vertical));

                    var color = GetRayColor(r, world);
                    pixelBuffer.SetPixelColor(i, j, color);
                }
            }

            return pixelBuffer;
        }

        private ColorVector GetRayColor(Ray ray, IHitable world)
        {
            HitRecord hr = world.Hit(ray, 0.0f, float.MaxValue);
            if (hr != null)
            {
                return (0.5f * new Vector3(hr.Normal.X + 1.0f, hr.Normal.Y + 1.0f, hr.Normal.Z + 1.0f)).ToColorVector();
            }
            else
            {
                var unitDirection = ray.Direction.ToUnitVector();
                float t = 0.5f * (unitDirection.Y + 1.0f);
                return (((1.0f - t) * Vector3.One) + (t * new Vector3(0.5f, 0.7f, 1.0f))).ToColorVector();
            }
        }
    }
}
