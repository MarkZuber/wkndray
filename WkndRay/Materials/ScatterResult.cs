﻿// -----------------------------------------------------------------------
// <copyright file="ScatterResult.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

namespace WkndRay.Materials
{
  public class ScatterResult
  {
    public ScatterResult(bool isScattered, ColorVector attenuation, Ray scatteredRay)
    {
      IsScattered = isScattered;
      Attenuation = attenuation;
      ScatteredRay = scatteredRay;
    }

    public static ScatterResult False()
    {
      return new ScatterResult(false, ColorVector.Zero, new Ray(PosVector.Zero, PosVector.Zero));
    }

    public bool IsScattered { get; }
    public ColorVector Attenuation { get; }
    public Ray ScatteredRay { get; }
  }
}