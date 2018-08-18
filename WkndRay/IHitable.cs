﻿// -----------------------------------------------------------------------
// <copyright file="IHitable.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay
{
  public interface IHitable
  {
    HitRecord Hit(Ray ray, double tMin, double tMax);
  }
}