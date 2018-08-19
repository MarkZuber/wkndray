﻿// -----------------------------------------------------------------------
// <copyright file="IMaterial.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public interface IMaterial
  {
    ScatterResult Scatter(Ray rayIn, HitRecord hitRecord);
    ColorVector Emitted(Point2D uvCoords, PosVector p);
  }
}