// -----------------------------------------------------------------------
// <copyright file="RandomService.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
  public class RandomService
  {
    private static readonly Random Random = new Random();
    private static readonly object Lock = new object();

    public static double NextDouble()
    {
      lock (Lock)
      {
        return Random.NextDouble();
      }
    }

    public static PosVector GetRandomCosineDirection()
    {
      double r1 = NextDouble();
      double r2 = NextDouble();
      double z = Math.Sqrt(1.0 - r2);
      double phi = 2.0 * Math.PI * r1;
      double x = Math.Cos(phi) * 2.0 * Math.Sqrt(r2);
      double y = Math.Sin(phi) * 2.0 * Math.Sqrt(r2);
      return new PosVector(x, y, z);
    }

    public static PosVector RandomToSphere(double radius, double distanceSquared)
    {
      double r1 = NextDouble();
      double r2 = NextDouble();
      double z = 1.0 + r2 * (Math.Sqrt(1.0 - radius * radius / distanceSquared) - 1.0);
      double phi = 2.0 * Math.PI * r1;
      double x = Math.Cos(phi) * Math.Sqrt(1.0 - z * z);
      double y = Math.Sin(phi) * Math.Sqrt(1.0 - z * z);
      return new PosVector(x, y, z);
    }
  }
}