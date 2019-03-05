// -----------------------------------------------------------------------
// <copyright file="VectorPerlin.cs" company="ZubeNET">
//   Copyright...
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace WkndRay
{
    public class VectorPerlin
    {
        private static readonly PosVector[] RanVector;
        private static readonly int[] PermX;
        private static readonly int[] PermY;
        private static readonly int[] PermZ;

        static VectorPerlin()
        {
            RanVector = PerlinGenerate();
            PermX = PerlinGenratePerm();
            PermY = PerlinGenratePerm();
            PermZ = PerlinGenratePerm();
        }

        public double Noise(PosVector p)
        {
            double u = p.X - Math.Floor(p.X);
            double v = p.Y - Math.Floor(p.Y);
            double w = p.Z - Math.Floor(p.Z);
            int i = Convert.ToInt32(Math.Floor(p.X));
            int j = Convert.ToInt32(Math.Floor(p.Y));
            int k = Convert.ToInt32(Math.Floor(p.Z));
            var c = new PosVector[2, 2, 2];
            for (int di = 0; di < 2; di++)
            {
                for (int dj = 0; dj < 2; dj++)
                {
                    for (int dk = 0; dk < 2; dk++)
                    {
                        c[di, dj, dk] = RanVector[PermX[(i + di) & 255] ^ PermY[(j + dj) & 255] ^ PermZ[(k + dk) & 255]];
                    }
                }
            }

            return PerlinInterpolate(c, u, v, w);
        }

        public double Turbulence(PosVector p, int depth = 7)
        {
            double accum = 0.0;
            PosVector tempP = p;
            double weight = 1.0;
            for (int i = 0; i < depth; i++)
            {
                accum += weight * Noise(tempP);
                weight *= 0.5;
                tempP *= 2.0;
            }

            return Math.Abs(accum);
        }

        private double PerlinInterpolate(PosVector[,,] c, double u, double v, double w)
        {
            double uu = u * u * (3.0 - (2.0 * u));
            double vv = v * v * (3.0 - (2.0 * v));
            double ww = w * w * (3.0 - (2.0 * w));
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
                        PosVector weightVec = new PosVector(u - dubi, v - dubj, w - dubk);
                        accum += ((dubi * uu) + ((1.0 - dubi) * (1.0 - uu))) *
                                 ((dubj * vv) + ((1.0 - dubj) * (1.0 - vv))) *
                                 ((dubk * ww) + ((1.0 - dubk) * (1.0 - ww))) *
                                 c[i, j, k].Dot(weightVec);
                    }
                }
            }

            return accum;
        }

        private static PosVector[] PerlinGenerate()
        {
            var p = new PosVector[256];
            for (int i = 0; i < 256; i++)
            {
                p[i] = new PosVector(
                  -1.0 + (2.0 * RandomService.NextDouble()),
                  -1.0 + (2.0 * RandomService.NextDouble()),
                  -1.0 + (2.0 * RandomService.NextDouble())).ToUnitVector();
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