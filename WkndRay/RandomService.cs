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
  }
}