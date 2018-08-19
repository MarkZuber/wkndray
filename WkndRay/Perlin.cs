// -----------------------------------------------------------------------
// <copyright file="Perlin.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
  public class Perlin
  {
    private static readonly double[] RanDouble;
    private static readonly int[] PermX;
    private static readonly int[] PermY;
    private static readonly int[] PermZ;

    static Perlin()
    {
      RanDouble = PerlinGenerate();
      PermX = PerlinGenratePerm();
      PermY = PerlinGenratePerm();
      PermZ = PerlinGenratePerm();
    }

    public double Noise(PosVector p, bool interpolate)
    {
      double u = p.X - Math.Floor(p.X);
      double v = p.Y - Math.Floor(p.Y);
      double w = p.Z - Math.Floor(p.Z);

      if (interpolate)
      {
        int i = Convert.ToInt32(Math.Floor(p.X));
        int j = Convert.ToInt32(Math.Floor(p.Y));
        int k = Convert.ToInt32(Math.Floor(p.Z));
        // Hermite cubic to round off the interpolation
        // to reduce Mach bands.
        u = u * u * (3.0 - 2.0 * u);
        v = v * v * (3.0 - 2.0 * v);
        w = w * w * (3.0 - 2.0 * w);

        var o = new double[2, 2, 2];
        for (int di = 0; di < 2; di++)
        {
          for (int dj = 0; dj < 2; dj++)
          {
            for (int dk = 0; dk < 2; dk++)
            {
              o[di, dj, dk] = RanDouble[PermX[(i + di) & 255] ^ PermY[(j + dj) & 255] ^ PermZ[(k + dk) & 255]];
            }
          }
        }

        return TrilinearInterpolate(o, u, v, w);
      }
      else
      {
        int i = Convert.ToInt32(4.0 * p.X) & 255;
        int j = Convert.ToInt32(4.0 * p.Y) & 255;
        int k = Convert.ToInt32(4.0 * p.Z) & 255;
        return RanDouble[PermX[i] ^ PermY[j] ^ PermZ[k]];
      }
    }

    private double TrilinearInterpolate(double[,,] o, double u, double v, double w)
    {
      double accum = 0.0;
      for (int i = 0; i < 2; i++)
      {
        double dubi = Convert.ToDouble(i);
        for (int j = 0; j < 2; j++)
        {
          double dubj = Convert.ToDouble(j);
          for (int k = 0; k < 2; k++)
          {
            double dubk = Convert.ToDouble(k);
            accum += (dubi * u + (1.0 - dubi) * (1.0 - u)) *
                     (dubj * v + (1.0 - dubj) * (1.0 - v)) *
                     (dubk * w + (1.0 - dubk) * (1.0 - w)) * o[i, j, k];
          }
        }
      }

      return accum;
    }

    private static double[] PerlinGenerate()
    {
      var p = new double[256];
      for (int i = 0; i < 256; ++i)
      {
        p[i] = RandomService.NextDouble();
      }

      return p;
    }

    private static int[] PerlinGenratePerm()
    {
      var p = new int[256];
      for (int i = 0; i < 256; ++i)
      {
        p[i] = i;
      }

      Permute(p);
      return p;
    }

    private static void Permute(int[] p)
    {
      for (int i = p.Length - 1; i > 0; i--)
      {
        int target = Convert.ToInt32(RandomService.NextDouble() * Convert.ToDouble(i + 1));
        int tmp = p[i];
        p[i] = p[target];
        p[target] = tmp;
      }
    }
  }
}